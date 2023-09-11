package com.TeamTwo.DatabaseProject.modules.user;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.PreparedStatement;
import java.util.ArrayList;
import java.util.Hashtable;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.TeamTwo.DatabaseProject.DatabaseBase;

@Service
public class UserDatabase extends DatabaseBase{

	@Autowired
	public UserDatabase(Connection db) {
		this.connection = db;
	}

	/**
	 * Example of getting info from a database for reference
	 * 
	 * @return ArrayList with: FirstName, Email
	 */
	public ArrayList<Hashtable<String, String>> TestQuery() {
		String query = """
					SELECT T.PersonId, T.FirstName, T.Email
					FROM
					(
						VALUES (1, N'Joe', N'Cool', N'joecool@ksu.edu'),
							(2, N'Jill', N'Cool', N'jillcool@ksu.edu')
					) T(PersonId, FirstName, LastName, Email);
				""";

		return sendQuery(query);
	}

	/*
	 * Gets all the messages from the given channel
	 * 
	 * @param ChannelId The ID number of the channel to get messages from
	 * 
	 * @return ArrayList->Hashtables - MsgId, Message, UpdatedOn, SenderId,
	 * FirstName, LastName, ProfilePhoto
	 */
	public ArrayList<Hashtable<String, String>> GetAllChannelMessages(int userId, int channelId) {
		String query = "EXEC Application.GetAllChannelMessages " + userId + ", " + channelId;
		System.out.println(query);
		return sendQuery(query);
	}

	/**
	 * Gets all direct messages between the two users with the given userIDs
	 * 
	 * @param userA The ID of the first user
	 * @param userB The ID of the second user
	 * @return ArrayList->Hashtables - MsgId, FirstName, LastName, Message,
	 *         UpdatedOn, SenderId, ProfilePhoto, IsMine
	 */
	public ArrayList<Hashtable<String, String>> GetDirectMessages(int userA, int userB) {
		String query = "EXEC Application.GetDirectMessages " + userA + "," + userB;
		return sendQuery(query);
	}

	/**
	 * Gets the profile photo of the given user
	 * 
	 * @param userId The user to get the profile photo for
	 * @return ArrayList->Hashtable - ProfilePhoto
	 */
	public ArrayList<Hashtable<String, String>> GetProfilePhoto(int userId) {
		String query = "EXEC Application.GetProfilePhoto " + userId;
		return sendQuery(query);
	}

	/**
	 * Gets the name of the channel with the given channelId
	 * 
	 * @param channelId The channelId of the channel to get the name of
	 * @return ArrayList->Hashtable - Name
	 */
	public ArrayList<Hashtable<String, String>> GetChannelName(int channelId) {
		String query = "EXEC Application.GetChannelName " + channelId;
		return sendQuery(query);
	}

	/**
	 * Gets the users in the given organization matching the given search string
	 * 
	 * @param substring      The name or email to search for
	 * @param organizationId The organization to search for
	 * @return ArrayList->Hashtables - UserId, FirstName, LastName, ProfilePhoto
	 */
	public ArrayList<Hashtable<String, String>> SearchUsersInOrganization(String substring, int organizationId) {
		//String query = "EXEC Application.SearchUsersInOrganization " + substring + "," + organizationId;
		//return sendQuery(query);
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String,String>>();
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.SearchUsersInOrganization ?,?")) {
			stmt.setString(1, substring);
			stmt.setInt(2, organizationId);
			ResultSet rs = stmt.executeQuery();
			int i = 0;
			int columns = rs.getMetaData().getColumnCount();
			// if (rs.isBeforeFirst()) results.get(0).add("Success");
			// else results.get(0).add("Empty");
			while (rs.next()) {
				Hashtable<String, String> m = new Hashtable<String, String>();
				results.add(m);

				for (int j = 1; j <= columns; j++) {
					String columnName = rs.getMetaData().getColumnName(j);
					results.get(i).put(columnName, rs.getString(j));

				}
				i++;
			}
			return results;
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
			return results;
		}
	}

	/**
	 * Gets all the channels that the given user is in
	 * 
	 * @param userId The user to get channels for
	 * @return ArrayList->Hashtable - ChannelId, Name
	 */
	public ArrayList<Hashtable<String, String>> GetAllChannelsOfUser(int userId) {
		String query = "EXEC Application.GetAllChannelsOfUser " + userId;
		return sendQuery(query);
	}

	/**
	 * Gets all the channels in that are in the given organization
	 * 
	 * @param organizationId The organization to get channels of
	 * @return ArrayList->Hashtable - ChannelId, Name
	 */
	public ArrayList<Hashtable<String, String>> GetAllChannelsInOrganization(int organizationId) {
		String query = "EXEC Application.GetAllChannelsInOrganization " + organizationId;
		return sendQuery(query);
	}

	/**
	 * Gets all the users that are in the given organization
	 * 
	 * @param organizationId The organization to get users of
	 * @return ArrayList - UserId, FirstName, LastName, ProfilePhoto
	 */
	public ArrayList<Hashtable<String, String>> GetAllUsersInOrganization(int organizationId) {
		String query = "EXEC Application.GetAllUsersInOrganization " + organizationId;
		return sendQuery(query);
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
	public Boolean InsertNewUser(int organizationId, String email, String firstName, String lastName, String title,
			String profilePhoto) {
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.InsertNewUser ?,?,?,?,?,?")) {
			stmt.setInt(1, organizationId);
			stmt.setString(2, email);
			stmt.setString(3, firstName);
			stmt.setString(4, lastName);
			stmt.setString(5, title);
			stmt.setString(6, profilePhoto);
			return stmt.execute();
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
		}
		return false;
	}
}
