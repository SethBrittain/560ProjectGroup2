using System.Text.Json;

namespace Pidgin.Util;

public class SendableMessage
{
    public int msgId { get; set; }
    public int senderId { get; set; }
    public int? channelId { get; set; } = null;
    public int? recipientId { get; set; } = null;
    public string message { get; set; }
    public DateTime dateSent { get; set; }
    public DateTime updatedOn { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string profilePhoto { get; set; }
    public bool isSender { get; set; }

    public SendableMessage(int msgId, int senderId, int channelId, string message, DateTime dateSent, DateTime updatedOn, string firstName, string lastName, string profilePhoto, bool isSender)
    {
        this.msgId = msgId;
        this.senderId = senderId;
        this.channelId = channelId;
        this.message = message;
        this.dateSent = dateSent;
        this.updatedOn = updatedOn;
        this.firstName = firstName;
        this.lastName = lastName;
        this.profilePhoto = profilePhoto;
        this.isSender = isSender;
    }

    public SendableMessage(int msgId, int senderId, string message, DateTime dateSent, DateTime updatedOn, string firstName, string lastName, string profilePhoto, bool isSender, int recipientId)
    {
        this.msgId = msgId;
        this.senderId = senderId;
        this.recipientId = recipientId;
        this.message = message;
        this.dateSent = dateSent;
        this.updatedOn = updatedOn;
        this.firstName = firstName;
        this.lastName = lastName;
        this.profilePhoto = profilePhoto;
        this.isSender = isSender;
    }
}
