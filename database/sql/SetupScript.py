import pymssql

path = __file__
path = path.removesuffix("SetupScript.py")
rootPath = path.removesuffix("database/sql/")
loginInfo = rootPath + "Backend/src/main/resources/application.properties"

with open(loginInfo, "r") as file:
    address = file.readline().split("=")[1].removesuffix("\n")
    user = file.readline().split("=")[1].removesuffix("\n")
    password = file.readline().split("=")[1].removesuffix("\n")
    port = file.readline().split("=")[1].removesuffix("\n")
    databaseName = file.readline().split("=")[1].removesuffix("\n")

script = []

setup = """IF SCHEMA_ID(N'Application') IS NULL
   EXEC(N'CREATE SCHEMA [Application];');

DROP TABLE IF EXISTS Application.Messages;
DROP TABLE IF EXISTS Application.Channels;
DROP TABLE IF EXISTS Application.Memberships;
DROP TABLE IF EXISTS Application.Groups;
DROP TABLE IF EXISTS Application.Users;
DROP TABLE IF EXISTS Application.Organizations;
DROP FUNCTION IF EXISTS Application.fn_CheckOrganizations"""

script.append(setup)

def getBatches(sql):
    with open(path + sql, "r") as file:
        batch = ""
        for line in file:
            if line.__contains__("GO"):
                script.append(batch)
                batch = ""
            else: batch += line

getBatches("Functions.sql")
getBatches("Tables.sql")
getBatches("Procedures.sql")
getBatches("Data/PopulateData.sql")
getBatches("Data/PopulateMessages.sql")

conn = pymssql.connect(server=address, user=user, password=password, database=databaseName)
cursor = conn.cursor()
num = script.__len__

for batch in script:
    cursor.execute(batch)
    conn.commit()
cursor.close()
conn.close()
