package com.TeamTwo.DatabaseProject.modules.user.controller;

import java.util.ArrayList;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

import com.TeamTwo.DatabaseProject.modules.user.database.UserDatabase;

@RestController
public class UserController {

    private UserDatabase database; 

    @Autowired
    public UserController(UserDatabase udb)
    {
        database = udb;
    }

    @GetMapping("/api/hello")
    public String hello()
    {
        database.testQuery();
        return "hello world & inserted test";
    }

	@GetMapping("/api/Example")
	public ArrayList<String> CollinTest()
	{
		return database.CollinTestQuery();
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
