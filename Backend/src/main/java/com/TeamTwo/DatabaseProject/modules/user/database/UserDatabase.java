package com.TeamTwo.DatabaseProject.modules.user.database;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.PreparedStatement;
import microsoft.sql.DateTimeOffset;
import java.util.ArrayList;
import java.util.Hashtable;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

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
	public ArrayList<Hashtable<String, String>> GetAllChannelMessages(int channelId) {
		String query = "EXEC Application.GetAllChannelMessages " + channelId;
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
	 * Gets all the users that the given user has direct messages with
	 * 
	 * @param userId The user to search for direct message chats with
	 * @return ArrayList->Hashtables - Message, SenderId, ChannelId, RecipientId,
	 *         CreatedOn, IsMine
	 */
	public ArrayList<Hashtable<String, String>> GetDirectMessageChats(int userId) {
		String query = "EXEC Application.GetDirectMessageChats " + userId;
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
		String query = "EXEC Application.SearchUsersInOrganization " + substring + "," + organizationId;
		return sendQuery(query);
	}

	/**
	 * Gets all the messages that match the given search string in the given channel
	 * 
	 * @param substring The substring to search for
	 * @param channelId The channel to get messages from
	 * @return ArrayList->Hashtables - Message, SenderId, ChannelId, RecipientId,
	 *         CreatedOn, IsMine
	 */
	public ArrayList<Hashtable<String, String>> SearchChannelMessages(int userId, int channelId, String subString) {
		String query = "EXEC Application.SearchChannelMessages " + userId + "," + channelId + "," + subString;
		return sendQuery(query);
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
	public ArrayList<Hashtable<String, String>> SearchUserMessages(int userId, String subString) {
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String, String>>();
		int i = 0;
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.SearchUserMessages ?,?")) {
			stmt.setInt(1,userId);
			stmt.setString(2,subString);
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
				System.out.println(results);
			}
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);

		}
		return results;

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

	// Insert Statements

	/**
	 * Inserts a channel message with the given parameters into the database
	 * 
	 * @param message   The content of the message to insert
	 * @param senderId  The userId of the sender of the message
	 * @param channelId The channelId to associate with the message
	 * @return Boolean - true if insertion was successful, false otherwise
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
	 * Inserts a direct message with the given parameters into the database
	 * 
	 * @param message     The content of the message to insert
	 * @param senderId    The userId of the sender of the message
	 * @param recipientId The userId of the receiver of the message
	 * @return Boolean - true if insertion was successful, false otherwise
	 */
	public Boolean InsertDirectMessage(String message, int senderId, int recipientId) {
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.InsertDirectMessage ?,?,?")) {
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
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.InsertNewUser ?,?,?,?,?,?")) {
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

	/**
	 * Deletes the message with the given messageId from the database
	 * 
	 * @param msgId The messageId to delete
	 * @return Boolean - true if the insertion is successful, false otherwise
	 */
	public Boolean DeleteMessage(int msgId) {
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.InsertNewUser ?")) {
			stmt.setInt(1, msgId);
			return stmt.execute();
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
		}
		return false;
	}

	/**
	 * Updates the content of the message with the given messageId to the given
	 * message content
	 * 
	 * @param msgId   The messageId of the message to update
	 * @param message The content to update the message with
	 * @return Boolean - true if the insertion is successful, false otherwise
	 */
	public Boolean UpdateMessage(int msgId, String message) {
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.InsertNewUser ?,?")) {
			stmt.setInt(1, msgId);
			stmt.setString(2, message);
			return stmt.execute();
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
		}
		return false;
	}

	// Aggregating queries

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

}
