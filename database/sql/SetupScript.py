import pymssql
import re

def partition_script(sql_script: str) -> list:
    """ Function will take the string provided as parameter and cut it on every line that contains only a "GO" string.
        Contents of the script are also checked for commented GO's, these are removed from the comment if found.
        If a GO was left in a multi-line comment, 
        the cutting step would generate invalid code missing a multi-line comment marker in each part.
    :param sql_script: str
    :return: list
    """
    # Regex for finding GO's that are the only entry in a line
    find_go = re.compile(r'^\s*GO\s*$', re.IGNORECASE | re.MULTILINE)
    # Regex to find multi-line comments
    find_comments = re.compile(r'/\*.*?\*/', flags=re.DOTALL)

    # Get a list of multi-line comments that also contain lines with only GO
    go_check = [comment for comment in find_comments.findall(sql_script) if find_go.search(comment)]
    for comment in go_check:
        # Change the 'GO' entry to '-- GO', making it invisible for the cutting step
        sql_script = sql_script.replace(comment, re.sub(find_go, '-- GO', comment))

    # Removing single line comments, uncomment if needed
    # file_content = re.sub(r'--.*$', '', file_content, flags=re.MULTILINE)

    # Returning everything besides empty strings
    return [part for part in find_go.split(sql_script) if part != '']


file = open("Tables.sql", "r")
tables = file.read()

file = open("Functions.sql", "r")
functions = file.read()

file = open("Procedures.sql", "r")
procedures = file.read()

file = open("Data/PopulateData.sql", "r")
data = file.read()

file = open("Data/PopulateMessages.sql", "r")
messages = file.read()

setup = """IF SCHEMA_ID(N'Application') IS NULL
   EXEC(N'CREATE SCHEMA [Application];');

DROP TABLE IF EXISTS Application.Messages;
DROP TABLE IF EXISTS Application.Channels;
DROP TABLE IF EXISTS Application.Memberships;
DROP TABLE IF EXISTS Application.Groups;
DROP TABLE IF EXISTS Application.Users;
DROP TABLE IF EXISTS Application.Organizations;
DROP FUNCTION IF EXISTS Application.fn_CheckOrganizations
GO"""


conn = pymssql.connect(server='172.17.0.2', user='SA', password='webkinz.com13', database='cohammo')
cursor = conn.cursor()
for batch in partition_script(tables):
    cursor.execute(batch)