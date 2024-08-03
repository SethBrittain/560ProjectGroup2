namespace Pidgin.Model;

public class DirectMessage {
	public int directMessageId { get; set; }
	public User sender { get; set; }
	public User recipient { get; set; }
	public string message { get; set; }
	public DateTime createdOn { get; set; }
	public DateTime updatedOn { get; set; }

	public DirectMessage(int directMessageId, User sender, User recipient, string message, DateTime createdOn, DateTime updatedOn)
	{
		this.directMessageId = directMessageId;
		this.sender = sender;
		this.recipient = recipient;
		this.message = message;
		this.createdOn = createdOn;
		this.updatedOn = updatedOn;
	}
}