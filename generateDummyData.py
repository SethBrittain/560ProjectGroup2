import random
import pymssql
import lorem
from random import randint

org_count = 5
group_count = 5
channel_count = 5
user_count = 5
message_count = 500
avg_memberships_per_user = 3
batch_size = 20

final_query = ''

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

	queryBase = f"INSERT INTO Application.{table_name}("

	# append column names
	for idx, name in enumerate(column_names):
		queryBase += name
		if idx != len(column_names)-1:
			queryBase += ','
		else:
			queryBase += ')\nVALUES '
	
	cur = queryBase
	for i in range(data):
		if i % batch_size == :


	# insert in batches of 1000
	for i in range(cycles):
		current = queryBase
		for j in range(batch_size):
			idx = (batch_size*i) + j
			if len(data[idx]) == 1:
				current += f"('{data[idx][0]}')"
			else:
				current += f'{data[idx]}'
			if idx != len(data)-1:
				current += ','
		result += current + ';\n|\n'
	
	# Insert remainder batch
	current = queryBase
	for i in range(remainder):
		idx = cycles*batch_size+i
		if len(data[idx]) == 1:
			current += f"('{data[idx][0]}')"
		else:
			current += f'{data[idx]}'
		if idx != len(data)-1:
			current += ','
	
	result += current + ';\n|\n'
	print(result)
	print(data)
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
		values.append((name,))
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
		username = f'{first_name[0]}{last_name}'.lower()
		email = f'{username}@example.com'
		password = hash(randint(1,99999999))
		organizationId = randint(1,org_count+1)
		role = 'User'
		title = random.choice(titles)
		avatar = f'https://robohash.org/{username}'
		values.append((first_name,last_name,username,email,password,organizationId,role,title,avatar))
	
	return get_insert_query('Users', ('Username','Email','Password','OrganizationId','RoleName','FirstName','LastName','Title','Active','ProfilePhoto'), values, user_count)

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
		values.append((randint(1,org_count+1),random.choice(group_names)))
	
	return get_insert_query('Groups', ('Name','OrganizationId'), values, group_count)

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
			values.append((channel_names[j], i+1))
	
	return get_insert_query('Channels', ('Name', 'GroupId'), values, channel_count)

def generate_messages():
	values = []
	for i in range(message_count):
		sender_id = randint(1,user_count+1)
		message = lorem.sentence()
		channel_id = randint(1,channel_count+1)
		
		values.append((sender_id, message, channel_id, None))
	return get_insert_query('Messages', ('SenderId', 'Message', 'ChannelId', 'RecipientId'), values, message_count)


def generate_memberships():
	values = []
	for i in range (avg_memberships_per_user*user_count):
		group_id = randint(1,group_count+1)
		user_id = randint(1,user_count+1)
		org_id = randint(1,org_count+1)
		values.append((group_id, user_id, org_id))
	return get_insert_query('Memberships', ('GroupId', 'UserId', 'OrganizationId'), values, avg_memberships_per_user*user_count)

def main():
	query = ''
	query += generate_orgs()
	query += generate_users()
	query += generate_groups()
	query += generate_channels()
	query += generate_messages()
	query += generate_memberships()
	
	queries = query.split('|')
	#for a in queries:
	#	print(a)
	'''
	with get_connection() as connection:
		cur = connection.cursor()
		cur.execute(query)
	'''
main()
