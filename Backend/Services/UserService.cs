
using pidgin.models;

namespace pidgin.services;
public sealed class UserService : IUserService
{

    private readonly NpgsqlConnection _connection;


    public UserService(NpgsqlConnection conn)
    {
        this._connection = conn;
    }


    public async Task<User> GetUserById(int id)
    {
        User? result = null;
        
        string sql = @"
            SELECT 
                u.user_id,
                organization_id,
                u.email,
                u.first_name,
                u.last_name,
                u.title,
                u.profile_photo,
                u.active,
                u.created_on,
                u.updated_on
            FROM users u
            WHERE u.user_id = @id";
        await using var command = new NpgsqlCommand(sql, _connection)
        {
            Parameters = 
            {
                new("id", id)
            }
        };

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
        while (await reader.ReadAsync())
        {
            if (!reader.HasRows) throw new Exception("User not found");
            
            result = new User
            (
                id: reader.GetInt32(0),
                organizationId: reader.GetInt32(1),
                email: reader.GetString(2),
                firstName: reader.GetString(3),
                lastName : reader.GetString(4),
                title: reader.GetString(5),
                profilePhotoUrl: reader.GetString(6),
                active: reader.GetBoolean(7),
                createdOn: reader.GetDateTime(8),
                updatedOn: reader.GetDateTime(9)
            );
        }
        

        if (result == null) throw new Exception("User not found");
        return result;
    }
    
    
    public async Task<User> GetUserByEmail(string email)
    {
        User? result = null;
        
        string sql = @"
            SELECT 
                u.user_id,
                organization_id,
                u.email,
                u.first_name,
                u.last_name,
                u.title,
                u.profile_photo,
                u.active,
                u.created_on,
                u.updated_on
            FROM users u
            WHERE u.email = @Email";
        await using var command = new NpgsqlCommand(sql, _connection)
        {
            Parameters = 
            {
                new("Email", email)
            }
        };

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
        while (await reader.ReadAsync())
        {
            if (!reader.HasRows) throw new Exception("User not found");
            
            result = new User
            (
                id: reader.GetInt32(0),
                organizationId: reader.GetInt32(1),
                email: reader.GetString(2),
                firstName: reader.GetString(3),
                lastName : reader.GetString(4),
                title: reader.GetString(5),
                profilePhotoUrl: reader.GetString(6),
                active: reader.GetBoolean(7),
                createdOn: reader.GetDateTime(8),
                updatedOn: reader.GetDateTime(9)
            );
        }
        

        if (result == null) throw new Exception("User not found");
        return result;
    }


    public void DeleteUser(int id)
    {
        throw new NotImplementedException();
    }


    public void UpdateUser(User user)
    {
        throw new NotImplementedException();
    }


    public int CreateUser(User user)
    {
        throw new NotImplementedException();
    }
}