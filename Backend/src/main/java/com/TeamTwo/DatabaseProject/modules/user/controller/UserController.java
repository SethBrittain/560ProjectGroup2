package com.TeamTwo.DatabaseProject.modules.user.controller;

import java.util.ArrayList;
import java.util.Hashtable;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
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
    public UserController(UserDatabase udb)
    {
        database = udb;
    }

	@GetMapping("/api/Example")
	public ArrayList<Hashtable<String,String>> TestQuery()
	{
		return database.TestQuery();
	}

	/**
	 * Gets data about all organizations in the database
	 * @return ArrayList<Object> - String OrgName, int ActiveUserCount, int MessageCount
	 */
	@GetMapping("/api/OrganizationsData")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetOrganizationData(@RequestParam DateTimeOffset startDate, @RequestParam DateTimeOffset endDate)
	{
		return database.GetOrganizationData(startDate, endDate);
	}

	/**
	 * Gets all the messages from the given channel
	 * @param ChannelId The ID number of the channel to get messages from
	 * @return ArrayList - Message
	 */
	@GetMapping("/api/GetAllChannelMessages")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetAllChannelMessages(@RequestParam int ChannelId)
	{
		return database.GetAllChannelMessages(ChannelId);
	}

	/**
	 * Gets all direct messages between the two users with the given userIDs
	 * @param userA The ID of the first user
	 * @param userB The ID of the second user
	 * @return ArrayList - Message
	 */
	@GetMapping("/api/DirectMessages")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetDirectMessages(@RequestParam int userA, @RequestParam int userB)
	{
		return database.GetDirectMessages(userA, userB);
	}

	@PutMapping("/api/GetAllChannelsInGroup")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetAllChannelsInGroup(@RequestParam int groupId)
	{
		return database.GetAllChannelsInGroup(groupId);
	}


	@GetMapping("/api/MessagesMatchingSubstring")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetAllMessagesMatchingSubstring(@RequestParam String substring, @RequestParam int channelId)
	{
		return database.GetAllMessagesMatchingSubstring(substring, channelId);
	}

	@GetMapping("/api/GetAllChannelsInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetAllChannelsInOrganization(@RequestParam int organizationId)
	{
		return database.GetAllChannelsInOrganization(organizationId);
	}

	@GetMapping("/api/GetAllUsersInOrganization")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetAllUsersInOrganization(@RequestParam int organizationId)
	{
		return database.GetAllUsersInOrganization(organizationId);
	}

	@GetMapping("/api/GetUserInfo")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetUserInfo(@RequestParam String username)
	{
		return database.GetUserInfo(username);
	}

	@PutMapping("/api/InsertMessageIntoChannel")
	@ResponseBody
	public Boolean InsertMessageIntoChannel(@RequestParam String message, @RequestParam int recipientId, @RequestParam String apiKey)
	{
		int senderId = 4; //method to convert api key into int
		return database.InsertMessageIntoChannel(message, senderId, recipientId);
	}

}
