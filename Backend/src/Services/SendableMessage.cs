using System.Text.Json;

namespace Pidgin.Services;

public class SendableMessage
{
    public int msgId { get; set; }
    public int senderId { get; set; }
    public int channelId { get; set; }
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
}
