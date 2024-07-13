using System.Data;
using Microsoft.VisualBasic;
using Npgsql;
using pidgin.models;
using Pidgin.Util;

namespace pidgin.services;
public sealed class MessageService : IMessageService
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly IUserService _userService;


    public MessageService(NpgsqlDataSource conn, IUserService userService)
    {
        this._dataSource = conn;
        this._userService = userService;
    }

    public async Task<Message> GetMessageById(int id)
    {
        Message result;
        
        string sql = @"
            SELECT 
                m.message_id,
                m.sender_id,
                m.channel_id,
                m.recipient_id,
                m.message,
                m.created_on,
                m.updated_on,
                m.is_deleted
            FROM public.messages m
            WHERE m.message_id = @id";
        await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
        
        reader.Read();
        if (!reader.HasRows) throw new Exception("User not found");
        
        User sender = await this._userService.GetUserById(reader.GetInt32(1));
        if (reader.IsDBNull(2))
        {
            User recipient = await this._userService.GetUserById(reader.GetInt32(3));
            result = new Message
            (
                messageId: reader.GetInt32(0),
                senderId: sender.id,
                recipientId: recipient.id,
                channelId: null,
                message: reader.GetString(4),
                createdOn: reader.GetDateTime(5),
                updatedOn: reader.GetDateTime(6),
                isDeleted: reader.GetBoolean(7)
            );
        }
        else
        {
            int channelId = reader.GetInt32(2);
            result = new Message
            (
                messageId: reader.GetInt32(0),
                senderId: sender.id,
                recipientId: null,
                channelId: channelId,
                message: reader.GetString(4),
                createdOn: reader.GetDateTime(5),
                updatedOn: reader.GetDateTime(6),
                isDeleted: reader.GetBoolean(7)
            );
        }

        if (result == null) throw new Exception("Message not found");
        return result;
    }

    public async Task<List<Message>> GetMessagesByChannelId(int channelId, int limit = 100)
    {
        List<Message> messages = new List<Message>();

        string sql = @"
            SELECT 
                m.message_id,
                m.sender_id,
                m.channel_id,
                m.recipient_id,
                m.message,
                m.created_on,
                m.updated_on,
                m.is_deleted
            FROM public.messages m
            WHERE m.channel_id = @id
            ORDER BY m.created_on ASC
            LIMIT @limit
        ";

        await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", channelId);
        command.Parameters.AddWithValue("limit", limit);
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            User sender = await this._userService.GetUserById(reader.GetInt32(1));
            
            messages.Add(new Message
            (
                messageId: reader.GetInt32(0),
                senderId: sender.id,
                recipientId: null,
                channelId: reader.GetInt32(2),
                message: reader.GetString(4),
                createdOn: reader.GetDateTime(5),
                updatedOn: reader.GetDateTime(6),
                isDeleted: reader.GetBoolean(7)
            ));
        }

        return messages;
    }

    public void DeleteMessage(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateMessage(Message m)
    {
        throw new NotImplementedException();
    }

    public async Task<Message> CreateChannelMessage(Message m)
    {
        if (m.channelId == null || m.updatedOn != null || m.createdOn != null) 
            throw new Exception("Message was either direct or already created");

        string sql = @"
            INSERT INTO messages
                (message, sender_id, channel_id)
            VALUES
                (@Message, @SenderId, @ChannelId)
            RETURNING *;
        ";

        await using (NpgsqlCommand cmd = this._dataSource.CreateCommand(sql))
        {
            cmd.Parameters.AddWithValue("Message", m.message);
            cmd.Parameters.AddWithValue("SenderId", m.senderId);
            cmd.Parameters.AddWithValue("ChannelId", m.channelId);

            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult))
            {
                if (!reader.HasRows) throw new Exception("Failed to create message");
                await reader.ReadAsync();
                
                return new Message(
                    messageId: reader.GetInt32(0),
                    senderId: m.senderId,
                    recipientId: null,
                    channelId: reader.GetInt32(2),
                    message: reader.GetString(4),
                    createdOn: reader.GetDateTime(5),
                    updatedOn: reader.GetDateTime(6),
                    isDeleted: reader.GetBoolean(7)
                );
            }
        }
    }

    public async Task<SendableMessage> CreateChannelMessageReturningSendable(Message m)
    {
        if (m.channelId == null || m.updatedOn != null || m.createdOn != null)
            throw new Exception("Message was either direct or already created");

        string sql = @"
            INSERT INTO messages
                (message, sender_id, channel_id)
            VALUES
                (@Message, @SenderId, @ChannelId)
            RETURNING message_id;
        ";

        int messageId;

        await using (NpgsqlCommand cmd = this._dataSource.CreateCommand(sql))
        {
            cmd.Parameters.AddWithValue("Message", m.message);
            cmd.Parameters.AddWithValue("SenderId", m.senderId);
            cmd.Parameters.AddWithValue("ChannelId", m.channelId);

            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                    messageId = reader.GetInt32(0);
                else
                    throw new Exception();
            }
        }

        string sql2 = @"
            SELECT 
                m.message_id, 
	            m.sender_id, 
	            m.channel_id, 
	            m.message, 
	            m.created_on, 
	            m.updated_on,
	            u.first_name,
	            u.last_name,
	            u.profile_photo
            FROM
                messages m
            LEFT JOIN users u
	            ON m.sender_id=u.user_id
            WHERE
                m.message_id=@messageId";

        await using (NpgsqlCommand cmd = this._dataSource.CreateCommand(sql2))
        {
            cmd.Parameters.AddWithValue("messageId", messageId);
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new SendableMessage(
                        reader.GetInt32(0),
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        reader.GetString(3),
                        reader.GetDateTime(4),
                        reader.GetDateTime(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8),
                        false
                    );
                }
                throw new Exception();
            }
        }
    }

    public async Task<SendableMessage> CreateDirectMessageReturningSendable(Message m)
    {
        if (m.recipientId == null || m.channelId == null || m.createdOn == null || m.updatedOn == null)
            throw new Exception("Direct message was not formatted properly");

        string sql = @"
            INSERT INTO messages
                (message, sender_id, recipient_id)
            VALUES
                (@Message, @SenderId, @RecipientId)
            RETURNING message_id;
        ";

        int messageId;

        await using (NpgsqlCommand cmd = this._dataSource.CreateCommand(sql))
        {
            cmd.Parameters.AddWithValue("Message", m.message);
            cmd.Parameters.AddWithValue("SenderId", m.senderId);
            cmd.Parameters.AddWithValue("RecipientId", m.recipientId);

            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                    messageId = reader.GetInt32(0);
                else
                    throw new Exception("Failed to insert direct message");
            }
        }

        string sql2 = @"
            SELECT 
                m.message_id, 
	            m.sender_id, 
	            m.recipient_id, 
	            m.message, 
	            m.created_on, 
	            m.updated_on,
	            u.first_name,
	            u.last_name,
	            u.profile_photo
            FROM
                messages m
            LEFT JOIN users u
	            ON m.sender_id=u.user_id
            WHERE
                m.message_id=@messageId";

        await using (NpgsqlCommand cmd = this._dataSource.CreateCommand(sql2))
        {
            cmd.Parameters.AddWithValue("messageId", messageId);
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new SendableMessage(
                        reader.GetInt32(0),
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        reader.GetString(3),
                        reader.GetDateTime(4),
                        reader.GetDateTime(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8),
                        false
                    );
                }
                throw new Exception();
            }
        }
    }

    public int CreateMessage(Message message)
    {
        throw new NotImplementedException();
    }

    public async Task<List<object>> GetAllChannelMessages(int channelId, int uid, int limit = 100)
    {
        List<object> result = new();

        string sql = @"
            SELECT 
                m.message_id, 
	            m.sender_id, 
	            m.channel_id, 
	            m.message, 
	            m.created_on, 
	            m.updated_on,
	            u.first_name,
	            u.last_name,
	            u.profile_photo,
                m.sender_id=@userId as is_mine
            FROM
                messages m
            LEFT JOIN users u
	            ON m.sender_id=u.user_id
            WHERE
                channel_id=@channelId AND is_deleted=FALSE
            ORDER BY created_on ASC
            LIMIT @limit OFFSET GREATEST (0,(
	            SELECT COUNT(*)-@limit FROM messages WHERE channel_id = @channelId
            )::integer);
        ";

        await using (NpgsqlCommand cmd = this._dataSource.CreateCommand(sql))
        {
            cmd.Parameters.AddWithValue("channelId", channelId);
            cmd.Parameters.AddWithValue("limit", limit);
            cmd.Parameters.AddWithValue("userId", uid);

            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    result.Add(new
                    {
                        MsgId = reader.GetInt32(0),
                        senderId = reader.GetInt32(1),
                        channelId = reader.GetInt32(2),
                        Message = reader.GetString(3),
                        dateSent = reader.GetDateTime(4),
                        UpdatedOn = reader.GetDateTime(5),
                        firstName = reader.GetString(6),
                        LastName = reader.GetString(7),
                        ProfilePhoto = reader.GetString(8)
                    });
                }
            }
            return result;
        }

    }
}