package com.TeamTwo.DatabaseProject.modules.user.controller;

import java.util.ArrayList;
import java.util.Hashtable;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

import com.TeamTwo.DatabaseProject.ApiConfig;
import com.TeamTwo.DatabaseProject.modules.user.database.UserDatabase;
import com.mashape.unirest.http.Unirest;
import com.mashape.unirest.http.exceptions.UnirestException;

import microsoft.sql.DateTimeOffset;

@RestController
public class UserController {

    private UserDatabase database; 
    private ApiConfig auth; 

    @Autowired
    public UserController(UserDatabase database, ApiConfig auth)
    {
        this.database = database;
		this.auth = auth;
    }

	private long GetUserId(String apiKey) {
		return this.database.GetUserId(apiKey);
	}

	@GetMapping("/api/Example")
	public ArrayList<Hashtable<String, String>> TestQuery() {
		return database.TestQuery();
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
	public ArrayList<Hashtable<String, String>> GetAllChannelMessages(@RequestParam int channelId) {
		return database.GetAllChannelMessages(channelId);
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
	public ArrayList<Hashtable<String, String>> GetDirectMessages(	@RequestParam int userBId) {
		// TODO Replace currentUserId with apiKey in parameters
		int userAId = 4;
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
	 * Gets the profile photo of the given user
	 * 
	 * @param userId The user to get the profile photo for
	 * @return ArrayList->Hashtable - ProfilePhoto
	 */
	@PostMapping("/api/GetProfilePhoto")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetProfilePhoto(@RequestParam int userId) {
		// TODO Replace userId with apiKey in parameters
		return database.GetProfilePhoto(userId);
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
		return database.GetChannelName(channelId);
	}

	/**
	 * Gets the users in the given organization matching the given search string
	 * 
	 * @param substring      The name or email to search for
	 * @param organizationId The organization to search for
	 * @return ArrayList->Hashtables - UserId, FirstName, LastName, ProfilePhoto
	 */
	@PostMapping("/api/SearchUsersInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> SearchUsersInOrganization(@RequestParam String subString,
			@RequestParam int organizationId) {
		return database.SearchUsersInOrganization(subString, organizationId);
	}

	/**
	 * Gets all the messages that match the given search string in the given channel
	 * 
	 * @param substring The substring to search for
	 * @param channelId The channel to get messages from
	 * @return ArrayList->Hashtables - Message, SenderId, ChannelId, RecipientId,
	 *         CreatedOn, IsMine
	 */
	@PutMapping("/api/SearchChannelMessages")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> SearchChannelMessages(@RequestParam int userId,
			@RequestParam int channelId, @RequestParam String subString) {
		// TODO Replace userId with apiKey in parameters
		return database.SearchChannelMessages(userId, channelId, subString);
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
			@RequestParam String subString) {
		// TODO Replace userId with apiKey in parameters
		int userId = 4;
		return database.SearchUserMessages(userId, subString);
	}

	/**
	 * Gets all the channels that the given user is in
	 * 
	 * @param userId The user to get channels for
	 * @return ArrayList->Hashtable - ChannelId, Name
	 */
	@PostMapping("/api/GetAllChannelsOfUser")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelsOfUser() {
		// TODO Replace userId with apiKey in parameters
		int userId = 4;
		return database.GetAllChannelsOfUser(userId);
	}

	/**
	 * Gets all the channels in that are in the given organization
	 * 
	 * @param organizationId The organization to get channels of
	 * @return ArrayList->Hashtable - ChannelId, Name
	 */
	@PutMapping("/api/GetAllChannelsInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelsInOrganization(@RequestParam int organizationId) {
		return database.GetAllChannelsInOrganization(organizationId);
	}

	/**
	 * Gets all the users that are in the given organization
	 * 
	 * @param organizationId The organization to get users of
	 * @return ArrayList - UserId, FirstName, LastName, ProfilePhoto
	 */
	@PostMapping("/api/GetAllUsersInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllUsersInOrganization(@RequestParam int organizationId) {
		return database.GetAllUsersInOrganization(organizationId);
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
	public Boolean InsertMessageIntoChannel(@RequestParam String message,
			@RequestParam int channelId) {
		// TODO Replace senderId with apiKey in parameterss
		int senderId = 4;
		return database.InsertMessageIntoChannel(message, senderId, channelId);
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
	public Boolean InsertDirectMessage( @RequestParam String message,
			@RequestParam int recipientId) {
				int senderId = 4;
		// TODO Replace senderId with apiKey in parameters
		return database.InsertDirectMessage(message, senderId, recipientId);
	}

	/**
	 * Inserts a new user with the given parameters into the database
	 * 
	 * @param orgId        The organization the user is part of
	 * @param email        The email address of the user
	 * @param firstName    The first name of the user
	 * @param lastName     The last name of the user
	 * @param title        The job title of the user
	 * @param profilePhoto The profile photo of the user
	 * @return Boolean - true if the insertion is successful, false otherwise
	 */
	@PutMapping("/api/InsertNewUser")
	@ResponseBody
	public Boolean InsertNewUser(@RequestParam int organizationId, @RequestParam String email,
			@RequestParam String firstName, @RequestParam String lastName, @RequestParam String title,
			@RequestParam String profilePhoto) {
		return database.InsertNewUser(organizationId, email, firstName, lastName, title, profilePhoto);
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

	// Aggregating Queries

	/**
	 * Gets data about all organizations in the database
	 * 
	 * @return ArrayList<Object> - String OrgName, int ActiveUserCount, int
	 *         MessageCount
	 */
	@PutMapping("/api/OrganizationsData")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetOrganizationData(@RequestParam DateTimeOffset startDate,
			@RequestParam DateTimeOffset endDate) {
		return database.GetOrganizationData(startDate, endDate);
	}

	@PutMapping("/api/CreateNewUser")
	@ResponseBody
	public Hashtable<String, String> CreateNewUser(@RequestParam String emailAddress, @RequestParam String firstName, @RequestParam String lastName, @RequestParam String authId) {
		String apiKey = database.CreateNewUser(emailAddress, firstName, lastName);
		this.SetUserAuthApiKey(apiKey, authId);
		Hashtable<String, String> hm = new Hashtable<String, String>();
		hm.put("ApiKey", apiKey);
		return hm;
	}

	private void SetUserAuthApiKey(String apiKey, String authId)
	{
		String dataBody = String.format("{\"app_metadata\": {\"api_key\": \"%s\"}}", apiKey);
		try {
			Unirest.patch(String.format("https://dev-nhscnbma.us.auth0.com/api/v2/users/%s",authId))
				.header("authorization", String.format("Bearer %s",this.auth.token()))
				.header("content-type", "application/json")
				.body(dataBody)
				.asJson();
		} catch (UnirestException e) {
			e.printStackTrace();
		}
	}
}
