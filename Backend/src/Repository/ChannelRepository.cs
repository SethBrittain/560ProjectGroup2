using Npgsql;
using Pidgin.Model;

namespace Pidgin.Repository;

public class ChannelRepository : ObjectRepository, IObjectRepository<Channel>
{
	public ChannelRepository(NpgsqlDataSource ds) : base(ds) { }

	public async Task<int> Create(Channel obj)
	{
		string sql = @"
			INSERT INTO channels(
				group_id,
				name
			) VALUES (
				@groupId,
				@name
			) RETURNING channel_id;
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("groupId", obj.groupId);
		command.Parameters.AddWithValue("name", obj.name);
		NpgsqlDataReader reader = await command.ExecuteReaderAsync();

		if (await reader.ReadAsync())
			return reader.GetInt32(0);
		
		throw new Exception("Failed to create channel");
	}

	public async Task Delete(int id, int uid)
	{
		string sql = @"
			DELETE FROM 
				channels
			WHERE 
				channel_id = @id
			AND 
				sender_id = @uid
		";

		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("id", id);
		command.Parameters.AddWithValue("uid", uid);

		if (await command.ExecuteNonQueryAsync() < 1)
			throw new Exception("Failed to delete channel");
	}

	public async Task<Channel> Get(int id, int uid)
	{
		string sql = @"
			SELECT DISTINCT
				c.channel_id,
				c.group_id,
				c.name,
				c.created_on,
				c.updated_on
			FROM channels c
			LEFT JOIN groups g 
				ON g.group_id = c.group_id
			LEFT JOIN memberships m 
				ON m.group_id = g.group_id
			WHERE 
				m.user_id = @uid AND c.channel_id = @channelId
		";
		
		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("channelId", id);
		command.Parameters.AddWithValue("uid", uid);

		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
			return new Channel (
				channelId: reader.GetInt32(0),
				groupId: reader.GetInt32(1),
				name: reader.GetString(2),
				createdOn: reader.GetDateTime(3),
				updatedOn: reader.GetDateTime(4)
			);
		
		throw new Exception("Channel not found");
	}

	public async Task<IEnumerable<Channel>> GetAll(int uid)
	{
		List<Channel> result = new();

		string sql = @"
			SELECT DISTINCT
				c.channel_id,
				c.group_id,
				c.name,
				c.created_on,
				c.updated_on
			FROM channels c
			LEFT JOIN groups g 
				ON g.group_id = c.group_id
			LEFT JOIN memberships m 
				ON m.group_id = g.group_id
			WHERE 
				m.user_id = @uid
			ORDER BY 
				c.channel_id ASC
		";
		
		await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
		command.Parameters.AddWithValue("uid", uid);
		await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
		while (await reader.ReadAsync())
			result.Add(new Channel(
				channelId: reader.GetInt32(0),
				groupId: reader.GetInt32(1),
				name: reader.GetString(2),
				createdOn: reader.GetDateTime(3),
				updatedOn: reader.GetDateTime(4)
			));
		
		if (result.Count == 0)
			throw new Exception("Channels not found");
		
		return result;
	}

	public Task Update(Channel obj, int uid)
	{
		throw new NotImplementedException();
	}
}