using System.Data;
using Npgsql;
using pidgin.Exceptions;
using pidgin.models;

namespace pidgin.services;

public class OrganizationService : IOrganizationService
{
	private NpgsqlDataSource _dataSource;

	public OrganizationService(NpgsqlDataSource dataSource)
	{
		this._dataSource = dataSource;
	}

	public async Task<List<User>> GetAllUsersInOrganization(Organization org, int limit = 10)
	{
		string sql = @"
			SELECT 
				u.user_id,
				u.organization_id,
				u.email,
				u.first_name,
				u.last_name,
				u.title,
				u.active,
				u.profile_photo,
				u.created_on,
				u.updated_on
			FROM users u
			WHERE u.organization_id = @id
			LIMIT @limit
		";
		await using (NpgsqlCommand command = _dataSource.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("id", org.organizationId);
			command.Parameters.AddWithValue("limit", limit);

			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
			{
				List<User> result = new();
				while (await reader.ReadAsync())
				{
					result.Add(new User
					(
						id: reader.GetInt32(0),
						organizationId: reader.GetInt32(1),
						email: reader.GetString(2),
						firstName: reader.GetString(3),
						lastName: reader.GetString(4),
						title: reader.GetString(5),
						active: reader.GetBoolean(6),
						profilePhotoUrl: reader.GetString(7),
						createdOn: reader.GetDateTime(8),
						updatedOn: reader.GetDateTime(9)
					));
				}
				return result;
			}
		
		}
	}

	public async Task<Organization> GetOrganizationById(long id)
	{
		string sql = @"
			SELECT 
				o.organization_id,
				o.name,
				o.active,
				o.created_on,
				o.updated_on
			FROM organizations o
			WHERE o.organization_id = @id
			LIMIT 1
		";
		await using (NpgsqlCommand command = _dataSource.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("id", id);
			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
			{
				if (!reader.HasRows) throw new OrganizationNotFoundException($"Organization with id {id} not found");
				reader.Read();
				return new Organization
				(
					organizationId: reader.GetInt32(0),
					name: reader.GetString(1),
					active: reader.GetBoolean(2),
					createdOn: reader.GetDateTime(3),
					updatedOn: reader.GetDateTime(4)
				);
			}
		}
	}
}