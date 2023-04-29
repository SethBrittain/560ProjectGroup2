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

@RestController
public class UserController {

	private UserDatabase database;
	private ApiConfig auth;

	@Autowired
	public UserController(UserDatabase database, ApiConfig auth) {
		this.database = database;
		this.auth = auth;
	}

	/**
	 * Gets the userId that matches the given apiKey
	 * @param apiKey The apiKey to get the userId for
	 * @return Int of a userId
	 */
	private int GetUserId(String apiKey) {
		return this.database.GetUserId(apiKey);
	}

	/**
	 * An example of how to use the API, returns an ArrayList of Hashtables with
	 * String-String key-value pairs which are the results of a query to the
	 * database
	 * 
	 * @return ArrayList->Hashtable - UserId, FirstName, Email
	 */
	@GetMapping("/api/Example")
	public ArrayList<Hashtable<String, String>> TestQuery() {
		String call = "{call Application.Example}";
		return database.callQueryProcedure(call, 0, null);
	}

	/**
	 * Gets all the messages for the given channel
	 * @param apiKey The apiKey of the user getting the messages
	 * @param channelId The channel to get messages for
	 * @return ArrayList->Hashtables - MsgId, Message, UpdatedOn, SenderId, FirstName, LastName, ProfilePhoto, IsMine
	 */
	@PostMapping("/api/GetAllChannelMessages")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelMessages(@RequestParam String apiKey, @RequestParam int channelId) {
		int userId = GetUserId(apiKey);
		String call = "{call Application.GetAllChannelMessages(?,?)}";
		Object[] args = { userId, channelId };
		return database.callQueryProcedure(call, 2, args);
	}

	/**
	 * Gets all direct messages between the two users with the given userIDs
	 * 
	 * @param userA The ID of the first user
	 * @param userB The ID of the second user
	 * @return ArrayList->Hashtables - MsgId, FirstName, LastName, Message,
	 *         UpdatedOn, SenderId, ProfilePhoto, IsMine
	 */
	@PostMapping("/api/GetDirectMessages")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetDirectMessages(@RequestParam String apiKey,
			@RequestParam int userBId) {
		int userAId = this.GetUserId(apiKey);
		String call = "{call GetDirectMessages(?,?)}";
		Object[] args = { userAId, userBId };
		return database.callQueryProcedure(call, 2, args);
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
	public ArrayList<Hashtable<String, String>> GetDirectMessageChats(@RequestParam String apiKey) {
		int userId = this.GetUserId(apiKey);
		String call = "{call GetDirectMessageChats(?)}";
		Object[] args = { userId };
		return database.callQueryProcedure(call, 1, args);
	}

	/**
	 * Gets the profile photo of the given user
	 * 
	 * @param userId The user to get the profile photo for
	 * @return ArrayList->Hashtable - FirstName - LastName - ProfilePhoto
	 */
	@PostMapping("/api/GetProfilePhoto")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetProfilePhoto(@RequestParam String apiKey) {
		int userId = this.GetUserId(apiKey);
		String call = "{call GetProfilePhoto(?)}";
		Object[] args = { userId };
		return database.callQueryProcedure(call, 1, args);
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
		String call = "{call GetChannelName(?)}";
		Object[] args = { channelId };
		return database.callQueryProcedure(call, 2, args);
	}

	/**
	 * Gets the users in the given organization matching the given search string
	 * 
	 * @param organizationId The organization to search for
	 * @param subString      The name or email to search for
	 * @return ArrayList->Hashtables - UserId, FirstName, LastName, ProfilePhoto
	 */
	@PostMapping("/api/SearchUsersInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> SearchUsersInOrganization(@RequestParam String organizationId,
			@RequestParam int subString) {
		String call = "{call SearchUsersInOrganization(?,?)}";
		Object[] args = { organizationId, subString };
		return database.callQueryProcedure(call, 2, args);
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
	public ArrayList<Hashtable<String, String>> SearchChannelMessages(@RequestParam String apiKey,
			@RequestParam int channelId, @RequestParam String subString) {
		int userId = this.GetUserId(apiKey);
		String call = "{call SearchChannelMessages(?,?,?)}";
		Object[] args = { userId, channelId, subString };
		return database.callQueryProcedure(call, 3, args);
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
	@PostMapping("/api/SearchUserMessages")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> SearchUserMessages(@RequestParam String apiKey,
			@RequestParam String subString) {
		int userId = this.GetUserId(apiKey);
		String call = "{call SearchUserMessages(?,?)}";
		Object[] args = { userId, subString };
		return database.callQueryProcedure(call, 2, args);
	}

	/**
	 * Gets all the channels that the given user is in
	 * 
	 * @param userId The user to get channels for
	 * @return ArrayList->Hashtable - ChannelId, Name
	 */
	@PostMapping("/api/GetAllChannelsOfUser")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelsOfUser(@RequestParam String apiKey) {
		int userId = this.GetUserId(apiKey);
		String call = "{call GetAllChannelsOfUser(?)}";
		Object[] args = { userId };
		return database.callQueryProcedure(call, 1, args);
	}

	/**
	 * Gets all the channels in that are in the given organization
	 * 
	 * @param organizationId The organization to get channels of
	 * @return ArrayList->Hashtable - ChannelId, Name
	 */
	@PostMapping("/api/GetAllChannelsInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelsInOrganization(@RequestParam int organizationId) {
		String call = "{call GetAllChannelsInOrganization(?)}";
		Object[] args = { organizationId };
		return database.callQueryProcedure(call, 1, args);
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
		String call = "{call GetAllUsersInOrganization(?)}";
		Object[] args = { organizationId };
		return database.callQueryProcedure(call, 1, args);
	}

	// Insertion statement API endpoints

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
	public Boolean InsertMessageIntoChannel(@RequestParam String apiKey, @RequestParam int channelId,
			@RequestParam String message) {
		int senderId = this.GetUserId(apiKey);
		String call = "{call InsertMessageIntoChannel(?,?,?)}";
		Object[] args = { senderId, channelId, message };
		return database.callStatementProcedure(call, 3, args);
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
	public Boolean InsertDirectMessage(@RequestParam String apiKey, @RequestParam int recipientId,
			@RequestParam String message) {
		int senderId = this.GetUserId(apiKey);
		String call = "{call InsertDirectMessage(?,?,?)}";
		Object[] args = { senderId, recipientId, message };
		return database.callStatementProcedure(call, 3, args);
	}

	/**
	 * Gets new direct messages after the given date between the given users
	 * @param apiKey The apiKey of the current user to get direct messages for
	 * @param userBId The userId of the user to get direct messages with for the current user
	 * @param sinceDateTime Date to check for messages after
	 * @return ArrayList->Hashtable - MsgId, Message, UpdatedOn, CreatedOn, SenderId, RecipientId, FirstName, LastName, ProfilePhoto, IsMine
	 */
	@PostMapping("/api/GetNewDirectMessages")
	public ArrayList<Hashtable<String, String>> GetNewDirectMessages(@RequestParam String apiKey,
			@RequestParam int userBId, @RequestParam String sinceDateTime) {
		int userId = this.GetUserId(apiKey);
		String call = "{call GetNewDirectMessage(?,?,?)}";
		Object[] args = { userId, userBId, sinceDateTime };
		return database.callQueryProcedure(call, 3, args);
	}

	/**
	 * Gets new channel messages after the given date in the given channel
	 * @param channelId the ID of the channel to get messages for
	 * @param sinceDateTime Date to check for messages after
	 * @return ArrayList->Hashtable - MsgId, Message, UpdatedOn, CreatedOn, SenderId, FirstName, LastName, ProfilePhoto
	 */
	@PostMapping("/api/GetNewChannelMessages")
	public ArrayList<Hashtable<String, String>> GetNewChannelMessages(@RequestParam int channelId,
			@RequestParam String sinceDateTime) {
		String call = "{call GetNewChannelMessages(?,?)}";
		Object[] args = { channelId, sinceDateTime };
		return database.callQueryProcedure(call, 2, args);
	}

	/**
	 * Inserts a new user with the given parameters into the database
	 * 
	 * @param organizationId        The organization the user is part of
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
		String call = "{call InsertNewUser(?,?,?,?,?,?)}";
		Object[] args = { organizationId, email, firstName, lastName, title, profilePhoto };
		return database.callStatementProcedure(call, 6, args);
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
		String call = "{call DeleteMessage(?)}";
		Object[] args = { msgId };
		return database.callStatementProcedure(call, 1, args);
	}

	/**
	 * Updates the message with the given messageId and new message from the
	 * database
	 * 
	 * @param msgId   The messageId to delete
	 * @param message The message to update
	 * @return Boolean - true if the insertion is successful, false otherwise
	 */
	@PutMapping("/api/UpdateMessage")
	@ResponseBody
	public Boolean UpdateMessage(@RequestParam int msgId, @RequestParam String message) {
		String call = "{call UpdateMessage(?,?)}";
		Object[] args = { msgId, message };
		return database.callStatementProcedure(call, 2, args);
	}

	// Aggregating Query API endpoints

	/**
	 * Gets usage stats about all the organizations in the database between the given dates
	 * @param startDate The date to start getting stats from
	 * @param endDate The
	 * @return
	 */
	@PostMapping("/api/OrganizationsData")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetOrganizationData(@RequestParam String startDate,
			@RequestParam String endDate) {
		String call = "{call GetOrganizationData(?,?)}";
		Object[] args = { startDate, endDate };
		return database.callQueryProcedure(call, 2, args);
	}

	@PostMapping("/api/GetGroupActivity")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetGroupActivity(@RequestParam int organizationId,
			@RequestParam String startDate, @RequestParam String endDate) {
		String call = "{call Application.GetGroupActivity(?,?,?)}";
		Object[] args = { organizationId, startDate, endDate };
		return database.callQueryProcedure(call, 3, args);
	}

	@PostMapping("/api/GetMonthlyTraffic")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetMonthlyTraffic(@RequestParam String startDate,
			@RequestParam String endDate) {
		String call = "{call GetMonthlyTraffic(?,?)}";
		Object[] args = { startDate, endDate };
		return database.callQueryProcedure(call, 1, args);
	}

	@PostMapping("/api/GetAppGrowth")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAppGrowth(@RequestParam String startDate,
			@RequestParam String endDate) {
		String call = "{call GetAppGrowth(?,?)}";
		Object[] args = { startDate, endDate };
		return database.callQueryProcedure(call, 2, args);
	}

	// API Key methods

	private void SetUserAuthApiKey(String apiKey, String authId) {
		String dataBody = String.format("{\"app_metadata\": {\"api_key\": \"%s\"}}", apiKey);
		try {
			Unirest.patch(String.format("https://dev-nhscnbma.us.auth0.com/api/v2/users/%s", authId))
					.header("authorization", String.format("Bearer %s", this.auth.token()))
					.header("content-type", "application/json")
					.body(dataBody)
					.asJson();
		} catch (UnirestException e) {
			e.printStackTrace();
		}
	}

	@PutMapping("/api/CreateUserOrGetKey")
	@ResponseBody
	public Hashtable<String, String> CreateUserOrGetKey(@RequestParam String emailAddress, @RequestParam String firstName, @RequestParam String lastName, @RequestParam String authId) {
		String apiKey = database.CreateUserOrGetKey(emailAddress, firstName, lastName, "");
		this.SetUserAuthApiKey(apiKey, authId);
		Hashtable<String, String> hm = new Hashtable<String, String>();
		hm.put("ApiKey", apiKey);
		return hm;
	}
}
