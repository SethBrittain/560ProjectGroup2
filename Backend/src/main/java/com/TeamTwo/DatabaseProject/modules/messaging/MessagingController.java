package com.TeamTwo.DatabaseProject.modules.messaging;

import java.util.ArrayList;
import java.util.Hashtable;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import com.TeamTwo.DatabaseProject.ControllerBase;

public class MessagingController extends ControllerBase<MessagingDatabase> {

    @Autowired
    public MessagingController(MessagingDatabase database)
    {
        this.database = database;
    }

    /**
	 * Gets all the messages from the given channel
	 * 
	 * @param ChannelId The ID number of the channel to get messages from
	 * @return ArrayList->Hashtables - MsgId, Message, UpdatedOn, SenderId,
	 *         FirstName, LastName, ProfilePhoto
	 */
	@PostMapping("/api/GetAllChannelMessages")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelMessages(@RequestParam String apiKey,@RequestParam int channelId) {
		int userId = GetUserId();
		return database.GetAllChannelMessages(userId, channelId);
	}
	
	/**
	 * Gets all direct messages between the two users with the given userIDs
	 * 
	 * @param userA The ID of the first user
	 * @param userB The ID of the second user
	 * @return ArrayList->Hashtables - MsgId, FirstName, LastName, Message,
	 *         UpdatedOn, SenderId, ProfilePhoto, IsMine
	 */
	//NEED TO ADD REQUEST PARAM FOR APIKEY
	@PostMapping("/api/GetDirectMessages")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetDirectMessages(	@RequestParam int userBId, @RequestParam String apiKey) {
		// TODO Replace currentUserId with apiKey in parameters
		int userAId = this.GetUserId();
		//int userAId = 1065;
		return database.GetDirectMessages(userAId, userBId);
	}

	/**
	 * Gets all the users that the given user has direct messages with
	 * 
	 * @param userId The user to search for direct message chats with
	 * @return ArrayList->Hashtables - Message, SenderId, ChannelId, RecipientId,
	 *         CreatedOn, IsMine
	 */
	@PostMapping("/api/GetDirectMessageChats")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetDirectMessageChats(@RequestParam int userId) {
		// TODO Replace userId with apiKey in parameters
		return database.GetDirectMessageChats(userId);
	}

	/**
	 * Gets all the messages sent to or from the given user that match the given
	 * search string
	 * 
	 * @param substring The message substring to search for
	 * @param userId    The userId to get messages for
	 * @return ArrayList->Hashtables - MsgId, Message, UpdatedOn, SenderId,
	 *         FirstName, LastName, ProfilePhoto, RecipientId, ChannelId, Name
	 */
	//NEED TO ADD REQUEST PARAM FOR APIKEY THEN CONVERT TO USER ID
	@PostMapping("/api/SearchUserMessages")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> SearchUserMessages(
			@RequestParam String subString,@RequestParam String apiKey) {
		// TODO Replace userId with apiKey in parameters
		int userId = this.GetUserId();
		//int userId = 1065;
		return database.SearchUserMessages(userId, subString);
	}

	/**
	 * Inserts a channel message with the given parameters into the database
	 * 
	 * @param message   The content of the message to insert
	 * @param senderId  The userId of the sender of the message
	 * @param channelId The channelId to associate with the message
	 * @return Boolean - true if insertion was successful, false otherwise
	 */
	@PutMapping("/api/InsertMessageIntoChannel")
	@ResponseBody
	public Boolean InsertMessageIntoChannel(@RequestParam String message, @RequestParam int channelId, @RequestParam String apiKey) {
		int senderId = this.GetUserId();
		return database.InsertMessageIntoChannel(message, senderId, channelId) == -1;
	}

	/**
	 * Inserts a direct message with the given parameters into the database
	 * 
	 * @param message     The content of the message to insert
	 * @param senderId    The userId of the sender of the message
	 * @param recipientId The userId of the receiver of the message
	 * @return Boolean - true if insertion was successful, false otherwise
	 */
	@PutMapping("/api/InsertDirectMessage")
	@ResponseBody
	public Boolean InsertDirectMessage( @RequestParam String message, @RequestParam int recipientId, @RequestParam String apiKey) {
		int senderId = this.GetUserId();
		return database.InsertDirectMessage(message, senderId, recipientId);
	}

	/**
	 * Deletes the message with the given messageId from the database
	 * 
	 * @param msgId The messageId to delete
	 * @return Boolean - true if the insertion is successful, false otherwise
	 */
	@PutMapping("/api/DeleteMessage")
	@ResponseBody
	public Boolean DeleteMessage(@RequestParam int msgId) {
		return database.DeleteMessage(msgId);
	}

	/**
	 * Updates the message with the given messageId and new message from the database
	 * 
	 * @param msgId The messageId to delete
	 * @param message The message to update
	 * @return Boolean - true if the insertion is successful, false otherwise
	 */
	@PutMapping("/api/UpdateMessage")
	@ResponseBody
	public Boolean UpdateMessage(@RequestParam int msgId, @RequestParam String message) {
		return database.UpdateMessage(msgId, message);
	}

	/**
	 * Gets all the messages that match the given search string in the given channel
	 * 
	 * @param substring The substring to search for
	 * @param channelId The channel to get messages from
	 * @return ArrayList->Hashtables - Message, SenderId, ChannelId, RecipientId,
	 *         CreatedOn, IsMine
	 */
	@PostMapping("/api/SearchChannelMessages")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> SearchChannelMessages(@RequestParam int channelId, @RequestParam String subString, @RequestParam String apiKey) {
		// TODO Replace userId with apiKey in parameters
		 int userId = this.GetUserId();
		//int userId = 1065;
		return database.SearchChannelMessages(userId, channelId, subString);
	}
    
    @PostMapping("/api/GetNewDirectMessages")
	public ArrayList<Hashtable<String,String>> GetNewDirectMessages(@RequestParam String sinceDateTime, @RequestParam String apiKey, @RequestParam String otherUserId)
	{
		int currentUser = this.GetUserId();
		int otherUserInt = Integer.parseInt(otherUserId);
		return this.database.GetNewDirectMessages(sinceDateTime, currentUser, otherUserInt);
	}

	@PostMapping("/api/GetNewChannelMessages")
	public ArrayList<Hashtable<String,String>> GetNewChannelMessages(@RequestParam String sinceDateTime, @RequestParam String channelId)
	{
		int chanId = Integer.parseInt(channelId);
		return this.database.GetNewChannelMessages(sinceDateTime, chanId);
	}

    /**
	 * Gets the name of the channel with the given channelId
	 * 
	 * @param channelId The channelId of the channel to get the name of
	 * @return ArrayList->Hashtable - Name
	 */
	@PostMapping("/api/GetChannelName")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetChannelName(@RequestParam int channelId) {
		return this.database.GetChannelName(channelId);
	}
}
