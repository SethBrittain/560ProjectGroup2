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

import com.TeamTwo.DatabaseProject.modules.user.database.UserDatabase;

import microsoft.sql.DateTimeOffset;

@RestController
public class UserController {

	private UserDatabase database;

	@Autowired
	public UserController(UserDatabase udb) {
		database = udb;
	}

	@GetMapping("/api/Example")
	public ArrayList<Hashtable<String, String>> TestQuery() {
		return database.TestQuery();
	}

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

	/**
	 * Gets all the messages from the given channel
	 * 
	 * @param ChannelId The ID number of the channel to get messages from
	 * @return ArrayList - Message
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
	 * @return ArrayList - Message
	 */
	@PostMapping("/api/GetDirectMessages")
	@ResponseBody
	// NEED TO ADD @RequestParam String APIkey
	public ArrayList<Hashtable<String, String>> GetDirectMessages(@RequestParam int userBId) {
		int userAId = 4; // get userAId from API key
		return database.GetDirectMessages(userAId, userBId);
	}

	@PutMapping("/api/GetAllChannelsInGroup")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelsInGroup(@RequestParam int groupId) {
		return database.GetAllChannelsInGroup(groupId);
	}

	@PutMapping("/api/MessagesMatchingSubstring")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllMessagesMatchingSubstring(@RequestParam String substring,
			@RequestParam int channelId) {
		return database.GetAllMessagesMatchingSubstring(substring, channelId);
	}

	@PutMapping("/api/GetAllChannelsInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelsInOrganization(@RequestParam int organizationId) {
		return database.GetAllChannelsInOrganization(organizationId);
	}

	@PutMapping("/api/GetAllUsersInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllUsersInOrganization(@RequestParam int organizationId) {
		return database.GetAllUsersInOrganization(organizationId);
	}

	@PutMapping("/api/GetUserInfo")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetUserInfo(@RequestParam String username) {
		return database.GetUserInfo(username);
	}

	// NEED TO ADD @RequestParam String APIkey
	@PutMapping("/api/InsertMessageIntoChannel")
	@ResponseBody
	public Boolean InsertMessageIntoChannel(@RequestParam String message, @RequestParam int channelId) {
		int senderId = 2; // method to convert api key into int
		return database.InsertMessageIntoChannel(message, senderId, channelId);
	}

	// NEED TO ADD @RequestParam String APIkey
	@PutMapping("/api/InsertDirectMessage")
	@ResponseBody
	public Boolean InsertDirectMessage(@RequestParam String message, @RequestParam int recipientId) {
		int senderId = 4; // method to convert api key into int
		return database.InsertDirectMessage(message, senderId, recipientId);
	}

	// NEED TO ADD @RequestParam String APIkey
	@PostMapping("/api/GetAllChannelsOfUser")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelsOfUser() {
		int userId = 9; // method to convert api key into int
		return database.GetAllChannelsOfUser(userId);
	}

}
