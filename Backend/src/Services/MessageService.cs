using System.Data;
using Microsoft.VisualBasic;
using Npgsql;
using pidgin.models;

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
                sender: sender,
                recipient: recipient,
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
                sender: sender,
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
                sender: sender,
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

    public int CreateMessage(Message m)
    {
        throw new NotImplementedException();
    }
}