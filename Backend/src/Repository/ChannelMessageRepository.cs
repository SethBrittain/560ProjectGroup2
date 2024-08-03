using Npgsql;
using Pidgin.Model;

namespace Pidgin.Repository;

public class ChannelMessageRepository : ObjectRepository, IObjectRepository<ChannelMessage>
{
	private readonly IObjectRepository<User> _userRepository;
	private readonly IObjectRepository<Channel> _channelRepository;
	public ChannelMessageRepository(NpgsqlDataSource ds, IObjectRepository<User> ur, IObjectRepository<Channel> cr) : base(ds)
	{ 
		_userRepository = ur;
		_channelRepository = cr;
	}

	public async Task<int> Create(ChannelMessage obj)
	{
		string sql = @"
			INSERT INTO channel_messages(
				sender_id, 
				channel_id, 
				message
			) VALUES (
				@sender_id, 
				@channel_id, 
				@message
			) RETURNING channel_message_id;";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("sender_id", obj.sender.id == null ? DBNull.Value : obj.sender.id);
		command.Parameters.AddWithValue("channel_id", obj.channel.channelId);
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
				channel_messages
			WHERE 
				channel_message_id = @id
			AND 
				sender_id = @uid
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", id);
		command.Parameters.AddWithValue("uid", uid);
		
		if (await command.ExecuteNonQueryAsync() < 1)
			throw new Exception("Failed to delete channel message");
	}

	public async Task<ChannelMessage> Get(int id, int uid)
	{
		// Gets a channel message by id, but only if the user is a member of the group that the channel belongs to
		string sql = @"
			SELECT DISTINCT 
			cm.channel_message_id, --0
				cm.message,
				cm.created_on,
				cm.updated_on,
			c.channel_id, --4
				c.group_id,
				c.name,
				c.created_on,
				c.updated_on,
			u.user_id, --9
				u.organization_id,
				u.email,
				u.first_name,
				u.last_name,
				u.title,
				u.active,
				u.profile_photo,
				u.created_on,
				u.updated_on
			FROM channel_messages cm
			LEFT JOIN channels c 
				ON c.channel_id = cm.channel_id
			LEFT JOIN groups g
				ON c.group_id = g.group_id
			LEFT JOIN memberships m 
				ON m.group_id = g.group_id
			LEFT JOIN users u
				ON u.user_id = cm.sender_id
			WHERE m.user_id=@uid AND cm.channel_message_id=@id
			ORDER BY cm.created_on ASC;
		";
		
		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", id);
		command.Parameters.AddWithValue("uid", uid);
		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
			return new ChannelMessage(
				channelMessageId: reader.GetInt32(0),
				message: reader.GetString(1),
				createdOn: reader.GetDateTime(2),
				updatedOn: reader.GetDateTime(3),
				new Channel(
					channelId: reader.GetInt32(4),
					groupId: reader.GetInt32(5),
					name: reader.GetString(6),
					createdOn: reader.GetDateTime(7),
					updatedOn: reader.GetDateTime(8)
				),
				new User(
					id: reader.GetInt32(9),
					organizationId: reader.GetInt32(10),
					email: reader.GetString(11),
					firstName: reader.GetString(12),
					lastName: reader.GetString(13),
					title: reader.IsDBNull(14) ? null : reader.GetString(14),
					active: reader.GetBoolean(15),
					profilePhotoUrl: reader.IsDBNull(16) ? null : reader.GetString(16),
					createdOn: reader.GetDateTime(17),
					updatedOn: reader.GetDateTime(18)
				)
			);
		else
			throw new Exception("Record not found");
	}

	public async Task<IEnumerable<ChannelMessage>> GetAll(int uid)
	{
		List<ChannelMessage> result = new List<ChannelMessage>();

		string sql = @"
			SELECT DISTINCT 
			cm.channel_message_id, --0
				cm.message,
				cm.created_on,
				cm.updated_on,
			c.channel_id, --4
				c.group_id,
				c.name,
				c.created_on,
				c.updated_on,
			u.user_id, --9
				u.organization_id,
				u.email,
				u.first_name,
				u.last_name,
				u.title,
				u.active,
				u.profile_photo,
				u.created_on,
				u.updated_on
			FROM channel_messages cm
			LEFT JOIN channels c 
				ON c.channel_id = cm.channel_id
			LEFT JOIN groups g
				ON c.group_id = g.group_id
			LEFT JOIN memberships m 
				ON m.group_id = g.group_id
			LEFT JOIN users u
				ON u.user_id = cm.sender_id
			WHERE m.user_id=@uid
			ORDER BY cm.created_on ASC;
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("uid", uid);
		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

		while (await reader.ReadAsync())
			result.Add(new ChannelMessage(
				channelMessageId: reader.GetInt32(0),
				message: reader.GetString(1),
				createdOn: reader.GetDateTime(2),
				updatedOn: reader.GetDateTime(3),
				new Channel(
					channelId: reader.GetInt32(4),
					groupId: reader.GetInt32(5),
					name: reader.GetString(6),
					createdOn: reader.GetDateTime(7),
					updatedOn: reader.GetDateTime(8)
				),
				new User(
					id: reader.GetInt32(9),
					organizationId: reader.GetInt32(10),
					email: reader.GetString(11),
					firstName: reader.GetString(12),
					lastName: reader.GetString(13),
					title: reader.IsDBNull(14) ? null : reader.GetString(14),
					active: reader.GetBoolean(15),
					profilePhotoUrl: reader.IsDBNull(16) ? null : reader.GetString(16),
					createdOn: reader.GetDateTime(17),
					updatedOn: reader.GetDateTime(18)
				)
			));
		
		if (result.Count == 0) throw new Exception("No records found");
		
		return result;
	}

	public async Task Update(ChannelMessage obj, int uid)
	{
		string sql = @"
			UPDATE channel_messages
			SET
				message = @message
			WHERE
				channel_message_id = @id
			AND
				sender_id = @uid
		";

		Console.WriteLine("obj.channelMessageId: " + obj.channelMessageId);
		Console.WriteLine("obj.message: " + obj.message);
		Console.WriteLine("uid: " + uid);

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", obj.channelMessageId);
		command.Parameters.AddWithValue("message", obj.message);
		command.Parameters.AddWithValue("uid", uid);
		
		await command.ExecuteNonQueryAsync();
	}
}