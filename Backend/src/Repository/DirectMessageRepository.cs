using Npgsql;
using Pidgin.Model;

namespace Pidgin.Repository;

public class DirectMessageRepository : ObjectRepository, IObjectRepository<DirectMessage>
{
	private readonly IObjectRepository<User> _userRepository;
	public DirectMessageRepository(NpgsqlDataSource ds, IObjectRepository<User> _userRepo) : base(ds) {
		_userRepository = _userRepo;
	 }

	public async Task<int> Create(DirectMessage obj)
	{
		string sql = @"
			INSERT INTO direct_messages(
				sender_id,
				recipient_id,
				message
			) VALUES (
				@senderId, 
				@recipientId, 
				@message
			) RETURNING direct_message_id;
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("senderId", obj.sender.id == null ? DBNull.Value : obj.sender.id);
		command.Parameters.AddWithValue("recipientId", obj.recipient.id == null ? DBNull.Value : obj.recipient.id);
		command.Parameters.AddWithValue("message", obj.message);
		NpgsqlDataReader reader = await command.ExecuteReaderAsync();

		if (await reader.ReadAsync())
			return reader.GetInt32(0);

		throw new Exception("Failed to create channel message");
	}

	public async Task Delete(int id, int uid)
	{
		string sql = @"
			DELETE FROM 
				direct_messages
			WHERE 
				direct_message_id = @id
			AND 
				sender_id = @uid
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", id);
		command.Parameters.AddWithValue("uid", uid);
		
		if (await command.ExecuteNonQueryAsync() < 1)
			throw new Exception("Failed to delete channel message");
	}

	public async Task<DirectMessage> Get(int id, int uid)
	{
		string sql = @"
			SELECT 
				dm.direct_message_id,
				dm.sender_id,
				dm.recipient_id,
				dm.message,
				dm.created_on,
				dm.updated_on 
			FROM 
				direct_messages dm
			WHERE 
				(dm.sender_id=@uid OR dm.recipient_id=@uid)
			AND
				dm.direct_message_id=@id
			ORDER BY dm.created_on ASC
		";
		
		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", id);
		command.Parameters.AddWithValue("uid", uid);
		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
		{
			return new DirectMessage(
				directMessageId: reader.GetInt32(0),
				sender: await _userRepository.Get(reader.GetInt32(1), uid),
				recipient: await _userRepository.Get(reader.GetInt32(2), uid),
				message: reader.GetString(3),
				createdOn: reader.GetDateTime(4),
				updatedOn: reader.GetDateTime(5)
			);
		}
		throw new Exception("Record not found");
	}

	public async Task<IEnumerable<DirectMessage>> GetAll(int uid)
	{
		List<DirectMessage> result = new List<DirectMessage>();

		string sql = @"
			SELECT
				dm.direct_message_id,
				s.user_id, --1
					s.organization_id,
					s.email,
					s.first_name,
					s.last_name,
					s.title,
					s.profile_photo,
					s.active,
					s.created_on,
					s.updated_on,
				r.user_id, --11
					r.organization_id,
					r.email,
					r.first_name,
					r.last_name,
					r.title,
					r.profile_photo,
					r.active,
					r.created_on,
					r.updated_on,
				dm.message, --21
				dm.created_on,
				dm.updated_on
			FROM direct_messages dm
			LEFT JOIN users s 
				ON s.user_id = dm.sender_id
			LEFT JOIN users r 
				ON r.user_id = dm.recipient_id
			WHERE 
				s.user_id=@uid OR r.user_id=@uid
			ORDER BY 
				dm.created_on ASC
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("uid", uid);
		
		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
		while (await reader.ReadAsync())
			result.Add(new DirectMessage(
				directMessageId: reader.GetInt32(0),
				sender: new User(
					id: reader.GetInt32(1),
					organizationId: reader.GetInt32(2),
					email: reader.GetString(3),
					firstName: reader.GetString(4),
					lastName: reader.GetString(5),
					title: reader.IsDBNull(6) ? null : reader.GetString(6),
					profilePhotoUrl: reader.IsDBNull(7) ? null : reader.GetString(7),
					active: reader.GetBoolean(8),
					createdOn: reader.GetDateTime(9),
					updatedOn: reader.GetDateTime(10),
					password: null
				),
				recipient: new User(
					id: reader.GetInt32(11),
					organizationId: reader.GetInt32(12),
					email: reader.GetString(13),
					firstName: reader.GetString(14),
					lastName: reader.GetString(15),
					title: reader.IsDBNull(16) ? null : reader.GetString(16),
					profilePhotoUrl: reader.IsDBNull(17) ? null : reader.GetString(17),
					active: reader.GetBoolean(18),
					createdOn: reader.GetDateTime(19),
					updatedOn: reader.GetDateTime(20),
					password: null
				),
				message: reader.GetString(21),
				createdOn: reader.GetDateTime(22),
				updatedOn: reader.GetDateTime(23)
			));
		
		if (result.Count == 0) throw new Exception("No records found");
		
		return result;
	}

	public async Task Update(DirectMessage obj, int uid)
	{
		string sql = @"
			UPDATE direct_messages
			SET
				message = @message
			WHERE
				direct_message_id = @id
			AND
				sender_id = @uid
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", obj.directMessageId);
		command.Parameters.AddWithValue("message", obj.message);
		command.Parameters.AddWithValue("uid", uid);
		
		await command.ExecuteNonQueryAsync();
	}
}