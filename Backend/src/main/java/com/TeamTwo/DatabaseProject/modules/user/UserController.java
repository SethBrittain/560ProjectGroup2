package com.TeamTwo.DatabaseProject.modules.user;

import java.util.ArrayList;
import java.util.Hashtable;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

import com.TeamTwo.DatabaseProject.ControllerBase;

@RestController
public class UserController extends ControllerBase<UserDatabase> {

    private UserDatabase database; 

    @Autowired
    public UserController(UserDatabase database)
    {
        this.database = database;
    }

	/**
	 * Gets the profile photo of the given user
	 * 
	 * @param userId The user to get the profile photo for
	 * @return ArrayList->Hashtable - FirstName - LastName - ProfilePhoto
	 */
	@PostMapping("/api/GetProfilePhoto")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetProfilePhoto(@RequestParam int userId) {
		// TODO Replace userId with apiKey in parameters
		return database.GetProfilePhoto(userId);
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
	 * Gets all the channels that the given user is in
	 * 
	 * @param userId The user to get channels for
	 * @return ArrayList->Hashtable - ChannelId, Name
	 */
	@PostMapping("/api/GetAllChannelsOfUser")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetAllChannelsOfUser(@RequestParam String apiKey) {
		// TODO Replace userId with apiKey in parameters
		int userId = this.GetUserId();
		//int userId = 1065;
		return database.GetAllChannelsOfUser(userId);
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
}
