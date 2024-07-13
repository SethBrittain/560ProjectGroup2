using System.Data;
using Npgsql;
using pidgin.models;
using Pidgin.Modules.Channels;

namespace pidgin.services;

public sealed class ChannelService : IChannelService
{
    private readonly NpgsqlDataSource _dataSource;

    public ChannelService(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public int CreateChannel(Channel channel)
    {
        throw new NotImplementedException();
    }

    public void DeleteChannel(int id)
    {
        throw new NotImplementedException();
    }

	public async Task<IEnumerable<Channel>> GetAllChannelsOfUser(int userId)
	{
		List<Channel> result = new();
        
        string sql = @"
            SELECT 
                c.channel_id,
                c.group_id,
                c.name,
                c.created_on,
                c.updated_on
            FROM channels c
            WHERE c.group_id IN 
			(
				SELECT 
					g.group_id
				FROM groups g
				WHERE g.organization_id = (SELECT u.organization_id FROM users u WHERE u.user_id = @id)
			) ORDER BY c.channel_id ASC
			LIMIT 10
		";

        await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", userId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
		{
			result.Add(new Channel
			(
				channelId: reader.GetInt32(0),
				groupId: reader.GetInt32(1),
				name: reader.GetString(2),
				createdOn: reader.GetDateTime(3),
				updatedOn: reader.GetDateTime(4)
			));
		}
        
        if (result == null) throw new Exception("Channels not found");
        return result;
	}

	public async Task<Channel> GetChannelById(int id)
    {
        Channel result;
        
        string sql = @"
            SELECT 
                c.channel_id,
                c.group_id,
                c.name,
                c.created_on,
                c.updated_on
            FROM channels c
            WHERE c.channel_id = @id";

        await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
        await reader.ReadAsync();
        
        if (!reader.HasRows) throw new Exception("Channel not found");
        
        IEnumerable<Message> messages = new List<Message>(); //= await this._channelService.GetMessagesByChannelId(id);

        result = new Channel
        (
            channelId: reader.GetInt32(0),
            groupId: reader.GetInt32(1),
            name: reader.GetString(2),
            createdOn: reader.GetDateTime(3),
            updatedOn: reader.GetDateTime(4)
        );
        
        if (result == null) throw new Exception("User not found");
        return result;
    }

    public void UpdateChannel(Channel channel)
    {
        throw new NotImplementedException();
    }
}