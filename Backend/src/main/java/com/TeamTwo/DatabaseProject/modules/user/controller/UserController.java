package com.TeamTwo.DatabaseProject.modules.user.controller;

import java.util.ArrayList;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
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
	public ArrayList<String> CollinTest()
	{
		return database.TestQuery();
	}

	/**
	 * Gets data about all organizations in the database
	 * @return ArrayList<Object> - String OrgName, int ActiveUserCount, int MessageCount
	 */
	@GetMapping("/api/OrganizationsData")
	@ResponseBody
	public ArrayList<String> GetOrgsData(@RequestParam DateTimeOffset startDate, @RequestParam DateTimeOffset endDate)
	{
		return database.GetOrgsData(startDate, endDate);
	}

	/**
	 * Gets all the messages from the given channel
	 * @param ChannelId The ID number of the channel to get messages from
	 * @return ArrayList - Message
	 */
	@GetMapping("/api/ChannelMessages")
	@ResponseBody
	public ArrayList<String> GetChannelMessages(@RequestParam int ChannelId)
	{
		return database.GetChannelMessages(ChannelId);
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


	@GetMapping("/api/Groups")
	@ResponseBody
	public ArrayList<String> GetAllGroupsInOrganization(@RequestParam String org)
	{
		return database.GetGroups(org);
	}

	@GetMapping("/api/Channels")
	@ResponseBody
	public ArrayList<String> GetAllChannelsInGroup(@RequestParam String org, @RequestParam String group)
	{
		return database.GetChannels(org, group);
	}
}
