import pymssql
import sys

class CreateFileGenerator:
    def __init__(self, config):
        self.conn = pymssql.connect(server=config['server'], \
            port='1433', \
            user=config['username'], \
            password=config['password'], \
            database=config['database_name'])
        print(f"Successfully connected to {config['database_name']}!")
        self.schemas = self.gather_schemas()
        self.objects = self.gather_objects()
        self.total = len(self.schemas)+len(self.objects)
        self.gen_count = 0
        self.cur_bar_count = 0

    def gather_objects(self):
        cursor = self.conn.cursor()
        cursor.execute('''
            SELECT o.name, 
            o.object_id, o.schema_id, o.parent_object_id, o.type_desc 
            FROM sys.all_objects o
            WHERE object_id > 0 
                AND is_ms_shipped=0
                AND o.type_desc <> 'DEFAULT_CONSTRAINT'
                AND o.type_desc <> 'PRIMARY_KEY_CONSTRAINT'
                AND object_id not in (
                    select major_id 
                    from sys.extended_properties 
                    where name = N'microsoft_database_tools_support' 
                    and minor_id = 0 
                    and class = 1
                )
            AND is_ms_shipped=0
        ''')
        return cursor.fetchall()

    def gather_schemas(self):
        schemas = {}
        cursor = self.conn.cursor()
        cursor.execute('''
            SELECT schema_id, name FROM sys.schemas WHERE schema_id < 16384
            AND schema_id > 4
        ''')
        results = cursor.fetchall()
        for result in results:
            schemas[result[0]] = result[1]
        return schemas

    def generate_create_string(self):
        result = """EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'\nGO\n\nEXEC sp_MSForEachTable 'DROP TABLE ?'\nGO\n\nEXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'\nGO\n\n"""
        result += self.generate_schema_string()
        result += self.generate_object_string()
        return result

    def generate_schema_string(self):
        result = ""
        for schema in self.schemas.values():
            result += f"CREATE SCHEMA IF NOT EXISTS {schema};\n" + "GO\n\n"
            self.gen_count += 1
            self.print_progress_bar()
        return result

    def construct_dependencies(self, object_id):
        object_id=2018106230
        cursor = self.conn.cursor()
        cursor.execute(f'''SELECT OBJECT_SCHEMA_NAME({object_id}), OBJECT_NAME({object_id})''')
        result = cursor.fetchone()
        param = str(result[0])+'.'+str(result[1])
        print(f'\n\n{result[1]} depends on:')
        cursor.callproc('sp_depends', (param,))
        cursor.fetchall()
        for row in cursor:
            print(f'{row}\n\n')

    def generate_object_string(self):
        result = ""
        for obj in self.objects:
            self.construct_dependencies(obj[1])
            objstr = self.get_string_from_obj(obj)
            if len(objstr) < 1:
                continue
            val = objstr + '\nGO\n\n'
            result += val
            self.gen_count += 1
            self.print_progress_bar()
        return result
    
    def get_string_from_obj(self, obj):
        match obj[4]:
            case 'USER_TABLE':
                return str(Table(obj[1], self.conn))
            case 'VIEW':
                return str(View(obj[1], self.conn))
            case 'SQL_STORED_PROCEDURE':
                return str(StoredProcedure(obj[1], self.conn))
            case 'UNIQUE_CONSTRAINT':
                return str(UniqueConstraint(obj[1], self.conn))
            case 'FOREIGN_KEY_CONSTRAINT':
                return str(ForeignKeyConstraint(obj[1], self.conn))
            case 'CHECK_CONSTRAINT':
                return str(CheckConstraint(obj[1], self.conn))
            case 'SQL_INLINE_TABLE_VALUED_FUNCTION':
                return str(TableValuedFunction(obj[1], self.conn))
            case _:
                self.cur_bar_count = -1
                txt = ' ERROR '
                print('*'*30 + txt + '*'*30)
                print(f'error obj_type of {obj[4]} not supported')
                print('*'*30 + '*'*(len(txt)) + '*'*30)
                return ''
    def print_progress_bar(self):
        val = self.gen_count/self.total
        perc = int(val*100)
        bars = int(val*25)
        if self.cur_bar_count != bars:
            self.cur_bar_count = bars
        prog = ''
        match bars%4:
            case 0:
                prog = '◜'
            case 1:
                prog = '◝'
            case 2:
                prog = '◞'
            case 3:
                prog = '◟'
        if bars == 25:
            prog = ''
        print('['+('='*(bars))+'>'+' '*(25-bars)+']'+f' {perc}% {prog}',end='\r',flush=True)
        if bars == 25:
            print('')

class Table:
    def __init__(self, object_id, conn):
        self.conn = conn
        cursor = conn.cursor()

        cursor.execute(f'''
            SELECT s.name AS schema_name, t.name AS table_name
            FROM sys.tables t
            INNER JOIN sys.schemas s
            ON s.schema_id=t.schema_id WHERE t.object_id={object_id};
        ''')
        
        self.table_properties = cursor.fetchone()
        self.schema_name = self.table_properties[0]
        self.table_name = self.table_properties[1]

        cursor.execute(f'''
            SELECT it.TABLE_SCHEMA, it.TABLE_NAME, 
            ic.COLUMN_NAME, ic.DATA_TYPE,
            c.[precision], c.max_length, 
            c.is_nullable, c.is_identity,
            ic.COLUMN_DEFAULT
            FROM sys.tables t 
            INNER JOIN sys.columns c ON c.object_id=t.object_id
            INNER JOIN INFORMATION_SCHEMA.TABLES it 
                ON it.TABLE_NAME=t.name 
                AND it.TABLE_SCHEMA = (
                    SELECT sds.name 
                    FROM sys.schemas sds 
                    WHERE sds.schema_id = t.schema_id
                )
            LEFT JOIN INFORMATION_SCHEMA.COLUMNS ic
                ON ic.TABLE_SCHEMA = it.TABLE_SCHEMA 
                AND ic.TABLE_NAME=it.TABLE_NAME AND ic.COLUMN_NAME=c.name
            WHERE t.object_id = {object_id}
        ''')
        self.data = cursor.fetchall()

    def __str__(self):
        result = f"CREATE TABLE {self.schema_name}.{self.table_name} (\n"
        count = 1
        for row in self.data:
            result += f'''    {row[2]} {row[3]}'''
            if row[5] == -1:
                temp = 'max'
            else:
                temp = row[5]
            match row[3]:
                case 'char':
                    result += f'({temp})'
                case 'varchar':
                    result += f'({temp})'
                case 'nvarchar':
                    result += f'({temp})'
                case 'nvarchar':
                    result += f'({temp})'
            if row[6] is False:
                result += ' NOT NULL'
            if row[7] is True:
                result += ' IDENTITY'
            if row[8] is not None:
                result += f' DEFAULT {row[8]}'
            if count != len(self.data):
                result += ',\n'
            else:
                result += '\n'
            count += 1
        result += ');'
        return result

class View:
    def __init__(self, object_id, conn):
        self.conn = conn
        self.object_id = object_id

    def __str__(self):
        cursor = self.conn.cursor()
        cursor.execute(f'''
            SELECT definition 
            FROM sys.all_sql_modules 
            WHERE object_id={self.object_id}
            AND definition IS NOT NULL
        ''')
        result = cursor.fetchone()
        if result is None:
            return ''
        return str(result[0])

class StoredProcedure:
    def __init__(self, object_id, conn):
        self.conn = conn
        self.object_id = object_id
    def __str__(self):
        cursor = self.conn.cursor()
        cursor.execute(f'''
            SELECT definition 
            FROM sys.all_sql_modules 
            WHERE object_id={self.object_id}
        ''')
        result = cursor.fetchone()[0]
        return str(result)

class ForeignKeyConstraint:
    def __init__(self, object_id, conn):
        self.object_id = object_id
        self.conn = conn
    def __str__(self):
        cursor = self.conn.cursor()
        cursor.execute(f'''
            SELECT 
            fk.name AS ConstraintName, 
            fk.update_referential_action_desc AS UpdateAction, 
            fk.delete_referential_action_desc AS DeleteAction, 
            (SELECT name FROM sys.schemas WHERE schema_id=t1.schema_id) AS ConstraintSchema,
            t1.name AS ConstraintTable,
            c1.name AS ConstraintTableColumn,
            (SELECT name FROM sys.schemas WHERE schema_id=t2.schema_id) AS ReferencedSchema,
            t2.name AS ReferencedTable,
            c2.name AS ConstraintTableColumn
            FROM sys.foreign_keys fk
            INNER JOIN sys.foreign_key_columns fkc
                ON fkc.constraint_object_id=fk.object_id
            INNER JOIN sys.columns c1
                ON c1.object_id=fkc.parent_object_id AND c1.column_id=fkc.constraint_column_id
            INNER JOIN sys.tables t1
                ON t1.object_id = c1.object_id
            INNER JOIN sys.columns c2
                ON c2.object_id=fkc.referenced_object_id AND c2.column_id=fkc.referenced_column_id
            INNER JOIN sys.tables t2
                ON t2.object_id = c2.object_id
            WHERE fk.object_id={self.object_id}
            ''')
        results = cursor.fetchone()
        
        result = f'ALTER TABLE {results[3]}.{results[4]}\n'
        result += f'ADD CONSTRAINT {results[0]}\n'
        result += f'FOREIGN KEY ({results[5]})'
        result += f' REFERENCES {results[6]}.{results[7]}({results[8]});'

        return result

class CheckConstraint:
    def __init__(self, object_id, conn):
        self.conn = conn
        self.object_id = object_id
    def __str__(self):
        cursor = self.conn.cursor()
        cursor.execute(f'''
            SELECT s.name, t.name, cc.name, cc.definition
            FROM sys.check_constraints cc
            INNER JOIN sys.tables t
                ON t.object_id=cc.parent_object_id
            INNER JOIN sys.schemas s
                ON t.schema_id=s.schema_id
            WHERE cc.object_id={self.object_id};
        ''')
        results = cursor.fetchone()
        if results[3] is None:
            return ''
        result = f'ALTER TABLE {results[0]}.{results[1]}\n'
        result += f'ADD CONSTRAINT {results[2]}\n'
        result += f'    CHECK {results[3]}'
        return str(result)

class UniqueConstraint:
    def __init__(self, object_id, conn):
        self.conn = conn
        self.object_id = object_id
    def __str__(self):
        cursor = self.conn.cursor()
        cursor.execute(f'''
            SELECT kc.name AS ConstraintName, 
                s.name AS TableSchemaName, 
                ccu.TABLE_NAME, 
                ccu.COLUMN_NAME
            FROM sys.key_constraints kc
            INNER JOIN sys.schemas s 
                ON s.schema_id=kc.schema_id
            INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
                ON ccu.CONSTRAINT_SCHEMA=s.name AND ccu.CONSTRAINT_NAME=kc.name
            WHERE kc.object_id={self.object_id}
            ''')
        results = cursor.fetchall()
        string=f'''ALTER TABLE {results[0][1]}.{results[0][2]}\nADD CONSTRAINT {results[0][0]} UNIQUE ('''
        count = 1
        for result in results:
            string += f'{result[3]}'
            if count != len(results):
                string += ','
            count += 1
        string += ');'

        return string

class TableValuedFunction:
    def __init__(self, object_id, conn):
        self.conn = conn
        self.object_id = object_id
    def __str__(self):
        cursor = self.conn.cursor()
        cursor.execute(f'''
            SELECT m.definition
            FROM sys.sql_modules m 
            WHERE m.object_id={self.object_id}
            AND definition IS NOT NULL
        ''')
        results = cursor.fetchone()
        if results is None:
            return ''
        else:
            return str(results[0]) or ''
