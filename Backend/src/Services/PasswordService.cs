using System.Text;
using Npgsql;
using Pidgin.Model;
using Pidgin.Repository;

namespace Pidgin.Services;

public class PasswordService : IPasswordService
{
	// ASCII 33-126
	private const char MIN_CHAR = '!';
	private const char MAX_CHAR = '~';
	
	private const short MIN_LENGTH = 8;
	private const short MAX_LENGTH = 52;

	private const short MIN_UNIQUE = 4;

	private const short WORK_FACTOR = 12;

	private readonly NpgsqlDataSource _dataSource;
	public PasswordService(NpgsqlDataSource ds)
	{
		this._dataSource = ds;
	}

	public string HashPassword(string password)
	{
		return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WORK_FACTOR);
	}

	public bool IsValidPassword(string password)
	{
		if (password.Length < MIN_LENGTH || password.Length > MAX_LENGTH)
			return false;
		if (password.Any(c => c < MIN_CHAR || c > MAX_CHAR))
			return false;
		if (password.Distinct().Count() < MIN_UNIQUE)
			return false;
		return true;
	}

	public async Task<bool> EmailExists(string email)
	{
		string sql = @"
			SELECT u.email 
			FROM users u 
			WHERE u.email = @email;
		";
		try 
		{
			await using NpgsqlCommand cmd = _dataSource.CreateCommand(sql);
			cmd.Parameters.AddWithValue("email", email);
			await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
			return reader.HasRows;
		} catch {
			return false;
		}
	}

	public async Task<int> ValidateCredentials(string email, string password)
	{
		bool exists = await EmailExists(email);
		if (!exists)
			return 0;
		
		string sql = @"
			SELECT u.password, u.user_id
			FROM users u
			WHERE u.email = @email
		";

		await using NpgsqlCommand cmd = _dataSource.CreateCommand(sql);
		cmd.Parameters.AddWithValue("email", email);
		NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult);

		string hash; int id;
		if (await reader.ReadAsync())
		{
			byte[] passwordHashBytes = new byte[60];
			reader.GetBytes(0, 0, passwordHashBytes, 0, 60);
			hash = Encoding.UTF8.GetString(passwordHashBytes);

			id = reader.GetInt32(1);
		}
		else throw new Exception("No user with that email found");

		return BCrypt.Net.BCrypt.Verify(password, hash) ? id : 0;
	}
}