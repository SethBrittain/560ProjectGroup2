using System.Text;
using Npgsql;
using Pidgin.Model;

namespace Pidgin.Repository;

public class UserRepository : ObjectRepository, IObjectRepository<User>
{

	public UserRepository(NpgsqlDataSource ds) : base(ds) { }

	public async Task<int> Create(User obj)
	{
		string sql = @"
			INSERT INTO users(
				organization_id,
				email,
				first_name,
				last_name,
				password
			) VALUES (
				@organizationId,
				@email,
				@firstName,
				@lastName,
				@password
			) RETURNING user_id;
		";
		if (obj.password == null) throw new Exception("Password is required");
		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("organizationId", obj.organizationId);
		command.Parameters.AddWithValue("email", obj.email);
		command.Parameters.AddWithValue("firstName", obj.firstName);
		command.Parameters.AddWithValue("lastName", obj.lastName);
		command.Parameters.AddWithValue("password", Encoding.UTF8.GetBytes(obj.password));
		NpgsqlDataReader reader = await command.ExecuteReaderAsync();

		string sql2 = @"
			INSERT INTO 
				memberships(user_id, group_id)
			SELECT 
				@uid, g.group_id
			FROM groups g
			WHERE g.organization_id=@oid
		";

		int uid = await reader.ReadAsync() ? reader.GetInt32(0) : throw new Exception("Failed to create user");
		
		await using NpgsqlCommand command2 = _dataSource.CreateCommand(sql2);
		command2.Parameters.AddWithValue("uid", uid);
		command2.Parameters.AddWithValue("oid", obj.organizationId);
		await command2.ExecuteNonQueryAsync();

		return uid;		
	}

	public async Task Delete(int id, int uid)
	{
		string sql = @"
			DELETE FROM 
				users
			WHERE 
				user_id = @id AND user_id = @uid
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", id);
		command.Parameters.AddWithValue("uid", uid);

		if (await command.ExecuteNonQueryAsync() < 1)
			throw new Exception("Failed to delete user");
	}

	public async Task<User> Get(int id, int uid)
	{
		string sql = @"
			SELECT DISTINCT 
				u2.user_id,
				u2.organization_id,
				u2.email,
				u2.first_name,
				u2.last_name,
				u2.title,
				u2.profile_photo,
				u2.active,
				u2.created_on,
				u2.updated_on
			FROM users u 
			RIGHT JOIN organizations o
				ON o.organization_id=u.organization_id
			RIGHT JOIN users u2
				ON u.organization_id=u.organization_id
			WHERE u.user_id = @uid AND u2.user_id = @id
			ORDER BY
				u2.created_on ASC
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", id);
		command.Parameters.AddWithValue("uid", uid);

		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
			return new User
			(
				id: reader.GetInt32(0),
				organizationId: reader.GetInt32(1),
				email: reader.GetString(2),
				firstName: reader.GetString(3),
				lastName: reader.GetString(4),
				title: reader.IsDBNull(5) ? null : reader.GetString(5),
				profilePhotoUrl: reader.IsDBNull(6) ? null : reader.GetString(6),
				active: reader.GetBoolean(7),
				createdOn: reader.GetDateTime(8),
				updatedOn: reader.GetDateTime(9)
			);
		else
			throw new Exception("User not found");
	}

	public async Task<IEnumerable<User>> GetAll(int uid)
	{
		List<User> result = new List<User>();

		string sql = @"
			SELECT DISTINCT 
				u2.user_id,
				u2.organization_id,
				u2.email,
				u2.first_name,
				u2.last_name,
				u2.title,
				u2.profile_photo,
				u2.active,
				u2.created_on,
				u2.updated_on
			FROM users u 
			RIGHT JOIN organizations o
				ON o.organization_id=u.organization_id
			RIGHT JOIN users u2
				ON u.organization_id=u.organization_id
			WHERE u.user_id = @uid
			ORDER BY
				u2.created_on ASC
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("uid", uid);
		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

		while (await reader.ReadAsync())
			result.Add(new User
			(
				id: reader.GetInt32(0),
				organizationId: reader.GetInt32(1),
				email: reader.GetString(2),
				firstName: reader.GetString(3),
				lastName: reader.GetString(4),
				title: reader.IsDBNull(5) ? null : reader.GetString(5),
				profilePhotoUrl: reader.IsDBNull(6) ? null : reader.GetString(6),
				active: reader.GetBoolean(7),
				createdOn: reader.GetDateTime(8),
				updatedOn: reader.GetDateTime(9)
			));

		if (result.Count == 0) throw new Exception("Users not found");
		return result;
	}

	public async Task Update(User obj, int uid)
	{
		string sql = @"
			UPDATE users
			SET
				organization_id = @organizationId,
				email = @email,
				first_name = @firstName,
				last_name = @lastName,
				title = @title,
				profile_photo = @profilePhoto,
				active = @active
			WHERE
				user_id = @uid;
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("organizationId", obj.organizationId);
		command.Parameters.AddWithValue("email", obj.email);
		command.Parameters.AddWithValue("firstName", obj.firstName);
		command.Parameters.AddWithValue("lastName", obj.lastName);
		command.Parameters.AddWithValue("title", obj.title == null ? DBNull.Value : obj.title);
		command.Parameters.AddWithValue("profilePhoto", obj.profilePhotoUrl == null ? DBNull.Value : obj.profilePhotoUrl);
		command.Parameters.AddWithValue("active", obj.active);
		command.Parameters.AddWithValue("uid", uid);

		if (await command.ExecuteNonQueryAsync() < 1)
			throw new Exception("Failed to update user");
	}
}