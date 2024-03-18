using System.Data;
using Npgsql;
using pidgin.Exceptions;
using pidgin.models;

namespace pidgin.services;
public sealed class UserService : IUserService
{
    private readonly NpgsqlDataSource _dataSource;

    public UserService(NpgsqlDataSource conn)
    {
        this._dataSource = conn;
    }

	public async void DeleteUser(User user)
	{
		string sql = @"
			DELETE FROM users u
			WHERE u.user_id = @id
		";

		await using (NpgsqlCommand command = _dataSource.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("id", user.id);
			int num = await command.ExecuteNonQueryAsync();
			if (num != 1) throw new Exception("Failed to delete user");
		}
	}

	public async Task<User> GetUserByEmail(string email)
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
			WHERE u.email = @email
		";
		await using (NpgsqlCommand command = _dataSource.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("email", email);
			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
			{
				if (!reader.HasRows) throw new UserNotFoundException($"User with email {email} not found");
				reader.Read();
				return new User
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
				);
			}
		}	
	}

	public async Task<User> GetUserById(long id)
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
			WHERE u.user_id = @id
		";
		await using (NpgsqlCommand command = _dataSource.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("id", id);
			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
			{
				if (!reader.HasRows) throw new UserNotFoundException($"User with id {id} not found");
				reader.Read();
				return new User
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
				);
			}
		}	
	
	}

	public async Task<User> RegisterUser(int orgId, string email, string firstName, string lastName, string title, string profilePhotoUrl)
	{
		string sql = @"
			INSERT INTO users
			(
				organization_id,
				email,
				first_name,
				last_name,
				title,
				active,
				profile_photo
			)
			VALUES
			(
				@orgId,
				@email,
				@firstName,
				@lastName,
				@title,
				@active,
				@profilePhotoUrl
			)
			RETURNING user_id
		";
		await using (NpgsqlCommand command = _dataSource.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("orgId", orgId);
			command.Parameters.AddWithValue("email", email);
			command.Parameters.AddWithValue("firstName", firstName);
			command.Parameters.AddWithValue("lastName", lastName);
			command.Parameters.AddWithValue("title", title);
			command.Parameters.AddWithValue("active", true);
			command.Parameters.AddWithValue("profilePhotoUrl", "");

			var ret = await command.ExecuteScalarAsync();
			if (ret == null) throw new Exception("Failed to insert new user");
			
			long id = (long)ret;
			return await GetUserById(id);
		}
	}

	public async Task<User> UpdateUser(User user)
	{
		string sql = @"
			UPDATE users u
			SET
				u.organization_id = @orgId,
				u.first_name = @firstName,
				u.last_name = @lastName,
				u.title = @title,
				u.active = @active,
				u.profile_photo = @profilePhotoUrl
			WHERE u.user_id = @id
			RETURNING id
		";
		await using (NpgsqlCommand command = _dataSource.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("orgId", user.organizationId);
			command.Parameters.AddWithValue("firstName", user.firstName);
			command.Parameters.AddWithValue("lastName", user.lastName);
			command.Parameters.AddWithValue("title", user.title);
			command.Parameters.AddWithValue("active", user.active);
			command.Parameters.AddWithValue("profilePhotoUrl", user.profilePhotoUrl);
			command.Parameters.AddWithValue("id", user.id);

			int? id = (int?)(await command.ExecuteScalarAsync());
			if (id == null) throw new Exception("Failed to update user");
			else return await GetUserById(id.Value);
		}
	}
}