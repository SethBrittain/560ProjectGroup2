using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Npgsql;

namespace pidgin.models;

public class Message
{
    /// <summary>
    /// The unique identifier for the message.
    /// </summary>
    public int messageId { get; private set; }

    /// <summary>
    /// The User who sent the message.
    /// </summary>
    public User sender { get; private set; }

    /// <summary>
    /// The unique identifier of the channel the message was sent in. Null if the message was sent directly to a user.
    /// </summary>
    public int? channelId { get; private set; }

    /// <summary>
    /// The user who received the message. Null if the message was sent in a channel.
    /// </summary>
    public User? recipient { get; private set; }

    /// <summary>
    /// The message content.
    /// </summary>
    public string message { get; private set; }

    /// <summary>
    /// The date the message was created.
    /// </summary>
    public DateTime? createdOn { get; private set; }

    /// <summary>
    /// The date the message was last updated.
    /// </summary>
    public DateTime? updatedOn { get; private set; }

    /// <summary>
    /// Whether the message has been deleted or not. Used for soft deletion.
    /// </summary>
    public bool isDeleted { get; private set; }

    public bool isChannelMessage => channelId != null;

    /// <summary>
    /// Constructor for a direct message
    /// </summary>
    public Message(int messageId, User sender, User recipient, string message, bool isDeleted, DateTime? createdOn = null, DateTime? updatedOn = null)
    {
        this.messageId = messageId;
        this.sender = sender;
        this.channelId = null;
        this.recipient = recipient;
        this.message = message;
        this.createdOn = createdOn;
        this.updatedOn = updatedOn;
        this.isDeleted = isDeleted;
    }
    
    /// <summary>
    /// Constructor for a channel message
    /// </summary>
    public Message(int messageId, User sender, int channelId, string message, bool isDeleted, DateTime? createdOn = null, DateTime? updatedOn = null)
    {
        this.messageId = messageId;
        this.sender = sender;
        this.recipient = null;
        this.channelId = channelId;
        this.message = message;
        this.createdOn = createdOn;
        this.updatedOn = updatedOn;
        this.isDeleted = isDeleted;
    }
}