import random
import pymssql
import lorem
from random import randint

org_count = 5
group_count = 5*5
channel_count = 5*5
user_count = 150
message_count = 1567
avg_memberships_per_user = 4
batch_size = 1000


def get_connection():
	try:
		config = {}
		with open('dbconfig.txt', 'r', encoding='UTF-8') as config_file:
			for line in config_file.readlines():
				split = line.split(':')
				config[split[0].strip()] = split[1].strip()
		return pymssql.connect(server=config['server'], \
            port='1433', \
            user=config['username'], \
            password=config['password'], \
            database=config['database_name'])
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

def get_insert_query(table_name, column_names, data, count):
	result = ''

	formatted_columns_names = '('
	for idx,col in enumerate(column_names):
		formatted_columns_names += col + ')' if idx == len(column_names)-1 else col + ','
	queryBase = f'INSERT INTO Application.{table_name}{formatted_columns_names} VALUES '

	cur = queryBase
	for i in range(len(data)):
		if (len(data[i]) == 1):
			cur += f'\n(\'{data[i][0]}\')'
		else:
			cur += f'\n{data[i]}'
		if i % batch_size == batch_size - 1 or i == len(data) - 1:
			cur += ';\n|\n'
			result += cur
			cur = queryBase
		else:
			cur+=','
	
	return result

def generate_orgs():
	names = [
		"Collins Inc",
		"Sawayn-Blanda",
		"Kuvalis Group",
		"Schmidt Group",
		"Hayes, Howell and Pagac",
		"Koss, Smitham and Kertzmann",
		"Gerlach-Jacobs",
		"Rowe, Kulas and Bradtke",
		"Cummerata Inc",
		"Mann-Hudson",
		"Kling Inc",
		"Abernathy-Green",
		"Gulgowski-Osinski",
		"Wilkinson, Koss and Lemke",
		"Ernser Group",
		"Hoppe-Cruickshank",
		"Murphy-Gottlieb",
		"Davis-McDermott",
		"Kuhn-Rutherford",
		"Gerlach-Hills",
		"Stiedemann-Ryan",
		"Huels LLC"
	]
	values = []
	for i in range(org_count):
		name = random.choice(names)
		values.append((name+str(i),))
	return get_insert_query('Organizations', {'Name'}, values, org_count)

def generate_users():
	names = (
		("Christiano","Popescu"),
		("Cobbie","Mannock"),
		("Simeon","Besson"),
		("Winthrop","Raithmill"),
		("Derril","Elloy"),
		("Luelle","Wroughton"),
		("Goraud","McBean"),
		("Carolin","Whiting"),
		("Ketty","Stainfield"),
		("Osmond","Bouttell"),
		("Edward","MacLoughlin"),
		("Cassandry","McGrae"),
		("Moishe","Guihen"),
		("Mort","Armer"),
		("Virgie","Thyng"),
		("Moselle","William"),
		("Ax","Kimm"),
		("Quent","Hanigan"),
		("Abbot","Scare"),
		("Lib","Castillou"),
		("Ronalda","Flewan")
	)
	titles = (
		"Social Worker",
		"Web Designer III",
		"Senior Quality Engineer",
		"Administrative Assistant II",
		"Dental Hygienist",
		"Recruiter",
		"Pharmacist",
		"Clinical Specialist",
		"Director of Sales",
		"Senior Editor",
		"Senior Financial Analyst",
		"Compensation Analyst",
		"General Manager",
		"VP Product Management",
		"Administrative Assistant III",
		"Financial Analyst",
		"Environmental Specialist",
		"Staff Scientist",
		"Staff Scientist",
		"Graphic Designer",
		"Mechanical Systems Engineer",
		"Software Test Engineer I",
		"Food Chemist",
		"Librarian",
		"GIS Technical Architect",
		"Administrative Officer",
		"Senior Sales Associate",
		"Office Assistant II",
		"Graphic Designer",
		"Legal Assistant",
		"Community Outreach Specialist",
		"Librarian",
		"Account Coordinator",
		"Chief Design Engineer",
		"Financial Analyst",
		"VP Quality Control",
		"Quality Control Specialist",
		"Nurse",
		"Data Coordinator",
		"Account Executive",
		"Human Resources Assistant IV",
		"Accounting Assistant II",
		"Sales Associate",
		"General Manager",
		"Internal Auditor",
		"Assistant Manager",
		"Developer IV",
		"Senior Cost Accountant",
		"Chief Design Engineer",
		"Actuary",
		"Information Systems Manager",
		"Developer II",
		"Information Systems Manager",
		"Nurse",
		"Project Manager",
		"Quality Engineer",
		"Internal Auditor",
		"Assistant Media Planner",
		"Senior Editor",
		"Business Systems Development Analyst",
		"Paralegal",
		"Recruiter",
		"Mechanical Systems Engineer",
		"VP Marketing",
		"Recruiting Manager",
		"Safety Technician II",
		"Paralegal",
		"Health Coach III",
		"Administrative Assistant IV",
		"Junior Executive",
		"Physical Therapy Assistant",
		"Junior Executive",
		"Health Coach III",
		"Account Executive",
		"GIS Technical Architect",
		"Actuary",
		"Sales Associate",
		"Product Engineer",
		"Cost Accountant",
		"Chief Design Engineer",
		"Dental Hygienist",
		"Software Consultant",
		"Quality Engineer",
		"Environmental Specialist",
		"Financial Analyst",
		"Nurse",
		"Electrical Engineer",
		"Legal Assistant",
		"Nurse",
		"Senior Developer",
		"Office Assistant III",
		"Data Coordinator",
		"Accountant I",
		"Recruiting Manager",
		"Physical Therapy Assistant",
		"Pharmacist",
		"Research Nurse",
		"Quality Control Specialist",
		"General Manager",
		"Computer Systems Analyst IV"
	)
	
	values = []
	for i in range(user_count):
		first_name = random.choice(names)[0]
		last_name = random.choice(names)[1]
		username = f'{first_name[0]}{last_name}'.lower() + str(i)
		email = f'{username}@example.com'
		password = str(hash(randint(1,99999999)))
		organizationId = randint(1,org_count+1)
		role = 'User'
		title = random.choice(titles)
		avatar = f'https://robohash.org/{username}'
		values.append((first_name,last_name,username,email,password,organizationId,role,title,avatar))
	
	return get_insert_query('Users', ('FirstName','LastName','Username','Email','Password','OrganizationId','RoleName','Title','ProfilePhoto'), values, user_count)

def generate_groups():
	group_names = (
		'Marketing',
		'Sales',
		'Design',
		'Management',
		'Human Resources',
		'Engineering',
		'Research and Development',
		'Quality Assurance',
		'Marketing',
		'Sales',
		'Design',
		'Management',
		'Human Resources',
		'Engineering',
		'Research and Development',
		'Quality Assurance'
	)

	values = []
	for i in range(group_count):
		values.append((randint(1,org_count),random.choice(group_names)+str(i)))
	
	return get_insert_query('Groups', ('OrganizationId','Name'), values, group_count)

def generate_channels():
	values = []
	channel_names = (
		'Announcements',
		'General',
		'Projects',
		'Help',
		'Issues'
	)
	for i in range(group_count):
		for j in range(len(channel_names)):
			values.append((channel_names[j]+str(i)+str(j), i+1))
	
	return get_insert_query('Channels', ('Name', 'GroupId'), values, channel_count)

def generate_messages():
	values = []
	for i in range(message_count):
		sender_id = randint(1,user_count+1)
		message = lorem.sentence()
		channel_id = randint(1,channel_count+1)
		
		values.append((sender_id, message, channel_id))
	return get_insert_query('Messages', ('SenderId', 'Message', 'ChannelId'), values, message_count)


def generate_memberships():
	values = []
	for i in range (avg_memberships_per_user*user_count):
		group_id = randint(1,group_count+1)
		user_id = randint(1,user_count+1)
		org_id = randint(1,org_count+1)
		if (group_id, user_id, org_id) not in values:
			values.append((group_id, user_id, org_id))
	return get_insert_query('Memberships', ('GroupId', 'UserId', 'OrganizationId'), values, avg_memberships_per_user*user_count)

def main():
	
	query = ''
	query += "INSERT INTO Application.Roles VALUES ('User')"
	query += generate_orgs()
	query += generate_groups()
	query += generate_users()
	query += generate_memberships()
	query += generate_channels()
	query += generate_messages()
	queries = query.split('|')
	
	with get_connection() as connection:
		c = connection.cursor()
		c.callproc('sp_MSforeachtable', ('ALTER TABLE ? NOCHECK CONSTRAINT ALL',))
		c.callproc('sp_MSforeachtable', ('DELETE FROM ?',))
		try:
			cur = connection.cursor()
			connection.commit()
			for q in queries:
				print(q)
				cur.execute(q)
				connection.commit()
		except Exception as e:
			raise e
		finally:
			c.callproc('sp_MSforeachtable', ('ALTER TABLE ? CHECK CONSTRAINT ALL ',))
			connection.close()


main()
