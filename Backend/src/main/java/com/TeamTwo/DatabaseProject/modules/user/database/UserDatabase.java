package com.TeamTwo.DatabaseProject.modules.user.database;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Dictionary;
import java.util.Hashtable;
import java.util.Map;

import javax.swing.RowFilter.Entry;

import java.sql.PreparedStatement;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import microsoft.sql.DateTimeOffset;

@Service
public class UserDatabase {

	private Connection database;

	@Autowired
	public UserDatabase(Connection db) {
		this.database = db;
	}

	/**
	 * Helper method to send a query to the database and parse the results for the
	 * given query
	 * 
	 * @param columns The number of columns the query will return
	 * @param query   The query to send to the database
	 * @return An ArrayList with the results of the query: "Empty" will be in the
	 *         first index if the results are empty, "Error" if there was an error
	 *         accessing the database
	 */
	private ArrayList<Hashtable<String, String>> sendQuery(String query) {
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String, String>>();
		int i = 0;
		try (PreparedStatement stmt = this.database.prepareStatement(query)) {
			ResultSet rs = stmt.executeQuery();
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
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);

		}
		return results;
	}

	public Boolean sendStatement(String query) {
		try (PreparedStatement stmt = this.database.prepareStatement(query)) {
			return stmt.execute();
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
		}
		return false;
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

	/**
	 * Gets data about all organizations in the database, counts messages for
	 * MessageCount from start to end
	 * 
	 * @return ArrayList - String OrgName, int ActiveUserCount, int MessageCount
	 */
	public ArrayList<Hashtable<String, String>> GetOrganizationData(DateTimeOffset start, DateTimeOffset end) {
		String query = "EXEC Application.GetOrganizationData " + start + " " + end;
		return sendQuery(query);
	}

	/**
	 * Gets all the messages from the given channel
	 * 
	 * @param ChannelId The ID number of the channel to get messages from
	 * @return ArrayList - Message, FirstName, LastName, CreatedOn
	 */
	public ArrayList<Hashtable<String, String>> GetAllChannelMessages(int ChannelId) {
		String query = "EXEC Application.GetAllChannelMessages " + ChannelId;
		return sendQuery(query);
	}

	/**
	 * Gets all direct messages between the two users with the given userIDs
	 * 
	 * @param userA The ID of the first user
	 * @param userB The ID of the second user
	 * @return ArrayList - Message, SenderId
	 */
	public ArrayList<Hashtable<String, String>> GetDirectMessages(int userA, int userB) {
		String query = "EXEC Application.GetDirectMessages " + userA + " " + userB;
		return sendQuery(query);
	}

	/**
	 * Get all messages that match a substring within a given channel
	 * 
	 * @param substring substring to match with
	 * @param channelId channel to search in
	 * @return ArrayList - Message, SenderId
	 */
<<<<<<< Updated upstream
<<<<<<< Updated upstream
	public ArrayList<Hashtable<String,String>> GetAllMessagesMatchingSubstring(String substring, int channelId)
	{
		String query = "EXEC Application.GetAllMessagesMatchingSubstring '" + substring + "'," + channelId;
		return sendQuery(query);		
=======
	public ArrayList<Hashtable<String, String>> GetAllMessagesMatchingSubstring(String substring, int channelId) {
		String query = "EXEC Application.GetAllMessagesMatchingSubstring " + substring + "," + channelId;
		return sendQuery(query);
>>>>>>> Stashed changes
=======
	public ArrayList<Hashtable<String, String>> GetAllMessagesMatchingSubstring(String substring, int channelId) {
		String query = "EXEC Application.GetAllMessagesMatchingSubstring " + substring + "," + channelId;
		return sendQuery(query);
>>>>>>> Stashed changes
	}

	/**
	 * Get all channels in an Organization
	 * 
	 * @param organizationId organization identification
	 * @return ArrayList - ChannelId, ChannelName
	 */
	public ArrayList<Hashtable<String, String>> GetAllChannelsInOrganization(int organizationId) {
		String query = "EXEC Application.GetAllChannelsInOrganization " + organizationId;
		return sendQuery(query);
	}

	/**
	 * Get all users in an organization
	 * 
	 * @param organizationId org identification
	 * @return ArrayList - UserId, FirstName, LastName, Username
	 */
	public ArrayList<Hashtable<String, String>> GetAllUsersInOrganization(int organizationId) {
		String query = "EXEC Application.GetAllUsersInOrganization " + organizationId;
		return sendQuery(query);
	}

	/**
	 * Get user info via username
	 * 
	 * @param email email
	 * @return ArrayList - Username, FirstName, LastName, Password, OrganizationId
	 */
	public ArrayList<Hashtable<String, String>> GetUserInfo(String username) {
		// Right now email doesn't work, but username does, the SQL engine doesn't like
		// the periods in email addresses
		String query = "EXEC Application.GetUserInfo " + username;
		return sendQuery(query);
	}

	/**
	 * Insert Message into channel
	 * 
	 * @param message   Message to insert
	 * @param senderId  sender id
	 * @param channelId channel id
	 */
	public Boolean InsertMessageIntoChannel(String message, int senderId, int channelId) {

		try (PreparedStatement stmt = this.database
				.prepareStatement("EXEC Application.InsertMessageIntoChannel ?,?,?")) {
			stmt.setString(1, message);
			stmt.setInt(2, senderId);
			stmt.setInt(3, channelId);
			return stmt.execute();
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
		}
		return false;

	}

	/**
	 * Insert direct message
	 * 
	 * @param message     message to insert
	 * @param senderId    sender id
	 * @param recipientId recipient id
	 */
	public Boolean InsertDirectMessage(String message, int senderId, int recipientId) {
		//String query = "EXEC Application.InsertDirectMessage " + message + " " + senderId + " " + recipientId;
		//return sendStatement(query);
		try (PreparedStatement stmt = this.database
				.prepareStatement("EXEC Application.InsertDirectMessage ?,?,?")) {
			stmt.setString(1, message);
			stmt.setInt(2, senderId);
			stmt.setInt(3, recipientId);
			return stmt.execute();
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
		}
		return false;
	}

	public ArrayList<Hashtable<String, String>> GetAllChannelsInGroup(int groupId) {
		String query = "EXEC Application.GetAllChannelsInGroup " + groupId;
		return sendQuery(query);
	}

	public ArrayList<Hashtable<String, String>> GetAllChannelsOfUser(int userId) {
		String query = "EXEC Application.GetAllChannelsOfUser " + userId;
		return sendQuery(query);
	}

}
