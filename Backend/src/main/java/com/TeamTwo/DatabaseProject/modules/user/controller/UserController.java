package com.TeamTwo.DatabaseProject.modules.user.controller;

import java.util.ArrayList;

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
	public ArrayList<String> TestQuery()
	{
		return database.TestQuery();
	}

	/**
	 * Gets data about all organizations in the database
	 * @return ArrayList<Object> - String OrgName, int ActiveUserCount, int MessageCount
	 */
	@GetMapping("/api/OrganizationsData")
	@ResponseBody
	public ArrayList<String> GetOrganizationData(@RequestParam DateTimeOffset startDate, @RequestParam DateTimeOffset endDate)
	{
		return database.GetOrganizationData(startDate, endDate);
	}

	/**
	 * Gets all the messages from the given channel
	 * @param ChannelId The ID number of the channel to get messages from
	 * @return ArrayList - Message
	 */
	@GetMapping("/api/ChannelMessages")
	@ResponseBody
	public ArrayList<String> GetAllChannelMessages(@RequestParam int ChannelId)
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
	public ArrayList<String> GetDirectMessages(@RequestParam int userA, @RequestParam int userB)
	{
		return database.GetDirectMessages(userA, userB);
	}

	@GetMapping("/api/GroupChannels")
	@ResponseBody
	public ArrayList<String> GetGroupChannels(@RequestParam int groupId)
	{
		return database.GetGroupChannels(groupId);
	}


	@GetMapping("/api/MessagesMatchingSubstring")
	@ResponseBody
	public ArrayList<String> GetAllMessagesMatchingSubstring(@RequestParam String substring, @RequestParam int channelId)
	{
		return database.GetAllMessagesMatchingSubstring(substring, channelId);
	}

	@GetMapping("/api/GetAllChannelsInOrganization")
	@ResponseBody
	public ArrayList<String> GetAllChannelsInOrganization(@RequestParam int organizationId)
	{
		return database.GetAllChannelsInOrganization(organizationId);
	}

	@GetMapping("/api/GetAllUsersInOrganization")
	@ResponseBody
	public ArrayList<String> GetAllUsersInOrganization(@RequestParam int organizationId)
	{
		return database.GetAllUsersInOrganization(organizationId);
	}

	@GetMapping("/api/GetUserInfo")
	@ResponseBody
	public ArrayList<String> GetUserInfo(@RequestParam String email)
	{
		return database.GetUserInfo(email); 
	}

	@PutMapping("/api/InsertMessageIntoChannel")
	@ResponseBody
	public void InsertMessageIntoChannel(@RequestParam String message, @RequestParam int senderId, @RequestParam int channelId){

		database.InsertMessageIntoChannel(message, senderId, channelId);
	}

	@PutMapping("/api/InsertDirectMessage")
	@ResponseBody
	public void InsertDirectMessage(@RequestParam String message, @RequestParam int senderId, @RequestParam int recipientId){

		database.InsertDirectMessage(message, senderId, recipientId);
	}



}
