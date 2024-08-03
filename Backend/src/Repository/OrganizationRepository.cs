using Npgsql;
using Pidgin.Model;

namespace Pidgin.Repository;

public class OrganizationRepository : ObjectRepository, IObjectRepository<Organization>
{
	public OrganizationRepository(NpgsqlDataSource ds) : base(ds) { }

	public async Task<int> Create(Organization obj)
	{
		string sql = @"
			INSERT INTO organizations(
				name
			) VALUES (
				@name
			) RETURNING organization_id;";
		
		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("name", obj.name);
		NpgsqlDataReader reader = await command.ExecuteReaderAsync();

		if (await reader.ReadAsync())
			return reader.GetInt32(0);

		throw new Exception("Failed to create organization");
	}

	public async Task Delete(int id, int uid)
	{
		string sql = @"
			DELETE FROM 
				organizations
			WHERE 
				organization_id = @id
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		if (await command.ExecuteNonQueryAsync() < 1)
			throw new Exception("Failed to delete organization");
	}

	public async Task<Organization> Get(int id, int uid)
	{
		string sql = @"
			SELECT
				o.organization_id,
				o.name,
				o.active,
				o.created_on,
				o.updated_on
			FROM 
				organizations o 
			LEFT JOIN 
				users u
			ON
				u.organization_id = o.organization_id
			WHERE 
				u.user_id=@uid AND o.organization_id=@id
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", id);

		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
			return new Organization
			(
				organizationId: reader.GetInt32(0),
				name: reader.GetString(1),
				active: reader.GetBoolean(2),
				createdOn: reader.GetDateTime(3),
				updatedOn: reader.GetDateTime(4)
			);
		else
			throw new Exception("Organization not found");
	}

	public async Task<IEnumerable<Organization>> GetAll(int uid)
	{
		List<Organization> result = new();

		string sql = @"
			SELECT
				o.organization_id,
				o.name,
				o.active,
				o.created_on,
				o.updated_on
			FROM 
				organizations o 
			LEFT JOIN 
				users u
			ON
				u.organization_id = o.organization_id
			WHERE 
				u.user_id=@uid
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("uid", uid);
		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

		while (await reader.ReadAsync())
			result.Add(new Organization
			(
				organizationId: reader.GetInt32(0),
				name: reader.GetString(1),
				active: reader.GetBoolean(2),
				createdOn: reader.GetDateTime(3),
				updatedOn: reader.GetDateTime(4)
			));

		if (result.Count == 0)
			throw new Exception("Organizations not found");

		return result;
	}

	public Task Update(Organization obj, int uid)
	{
		throw new NotImplementedException();
	}
}