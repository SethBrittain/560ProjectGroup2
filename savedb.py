"""
Driver for connecting to the database
"""
import time
from database import CreateFileGenerator

config = {}
try:
    with open('dbconfig.txt', 'r', encoding='UTF-8') as config_file:
        for line in config_file.readlines():
            split = line.split(':')
            config[split[0].strip()] = split[1].strip()
except FileNotFoundError:
    result = 'No database configuration file was found!\n\n'

    result += 'You need a database configuration file in order to \n'
    result += 'connect to your personal database so that you can \n'
    result += 'sync your changes and track them in version control.\n\n'

    result += 'This script will generate an sql file used to configure the \n'
    result += 'database in the same way that the "Production" database is \n'
    result += 'configured.\n\n'

    result += 'For this to work, you need to create a file in the top-level \n'
    result += 'directory named dbconfig.txt \n'
    result += 'with this format:\n\n'
 
    result += 'server:server.address.com\n'
    result += 'username:sbrittain\n'
    result += 'password:Secr3tP4ssword!\n'
    result += 'database_name:nameofdb'

    print(result)
    exit()


proc_time = time.time()
file_gen = CreateFileGenerator(config)
create_string = file_gen.generate_create_string()
exec_time = time.time()-proc_time

if (len(create_string) < 1):
    print('failed to retrieve create string')
else:
    print(f'Generation finished in {exec_time:.4f} seconds')
    with open("setup.sql", "w", encoding='UTF-8') as text:
        text.write(create_string)

