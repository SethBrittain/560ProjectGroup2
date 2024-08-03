namespace Pidgin.Model;

public class ChannelMessage {
	public int channelMessageId { get; set; }
	public User sender { get; set; }
	public Channel channel { get; set; }
	public string message { get; set; }
	public DateTime createdOn { get; set; }
	public DateTime updatedOn { get; set; }

	public ChannelMessage(int channelMessageId, string message, DateTime createdOn, DateTime updatedOn, Channel channel, User sender)
	{
		this.channelMessageId = channelMessageId;
		this.sender = sender;
		this.channel = channel;
		this.message = message;
		this.createdOn = createdOn;
		this.updatedOn = updatedOn;
	}
}