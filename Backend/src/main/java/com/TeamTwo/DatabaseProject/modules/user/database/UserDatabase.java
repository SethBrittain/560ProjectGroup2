package com.TeamTwo.DatabaseProject.modules.user.database;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.PreparedStatement;
import microsoft.sql.DateTimeOffset;
import java.util.ArrayList;
import java.util.Hashtable;

import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.TeamTwo.DatabaseProject.exceptions.ApiException;

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
		//String query = "EXEC Application.SearchUsersInOrganization " + substring + "," + organizationId;
		//return sendQuery(query);
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String,String>>();
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.SearchUsersInOrganization ?,?")) {
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
	 * @return int - id of the inserted message
	 */
	public int InsertMessageIntoChannel(String message, int senderId, int channelId) {
		try {
			String insert = "INSERT INTO Application.Messages(Message, SenderId, ChannelId) VALUES (?, ?, ?)";
			PreparedStatement stmt = this.database.prepareStatement(insert, PreparedStatement.RETURN_GENERATED_KEYS);
			stmt.setString(1, message);
			stmt.setInt(2, senderId);
			stmt.setInt(3, channelId);
			stmt.executeUpdate();
			
			ResultSet generatedKeys = stmt.getGeneratedKeys();
			if (generatedKeys.next()) {
				System.out.println(generatedKeys.getInt(1));
				return generatedKeys.getInt(1);
			}
			else {
				throw new SQLException("Creating failed, no ID obtained.");
			}
			
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
			return -1;
		}
	}

	public JSONObject GetMessageById(Integer msgId, Integer uid) throws SQLException {
		try
		{
			String call = "{call Application.GetMessageById(?,?)}";
			Object[] args = { uid.toString(), msgId.toString() };

			Hashtable<String,String> message = this.callQueryProcedure(call, 2, args).get(0);
			JSONObject result = new JSONObject();
			
			result.put("MsgId", message.get("MsgId"));
			result.put("Message", message.get("Message"));
			result.put("UpdatedOn", message.get("UpdatedOn"));
			result.put("SenderId", message.get("SenderId"));
			result.put("FirstName", message.get("FirstName"));
			result.put("LastName", message.get("LastName"));
			result.put("ProfilePhoto", message.get("ProfilePhoto"));
			result.put("IsMine", message.get("IsMine"));
			return result;
		} catch (Exception e) {
			System.out.println(e.getMessage());
			throw e;
		}
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
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.DeleteMessage ?")) { // InsertNewUser
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
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.UpdateMessage ?,?")) {
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
	public ArrayList<Hashtable<String, String>> GetOrganizationData(String start, String end) {
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String,String>>();
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.GetOrganizationData ?,?")) {
			stmt.setString(1, start);
			stmt.setString(2, end);
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

	public ArrayList<Hashtable<String,String>> GetMonthlyTraffic(String start, String end){
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String,String>>();
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.GetMonthlyTraffic ?,?")) {
			stmt.setString(1, start);
			stmt.setString(2, end);
			ResultSet rs = stmt.executeQuery();
			int i = 0;
			int columns = rs.getMetaData().getColumnCount();
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

	public ArrayList<Hashtable<String,String>> GetAppGrowth(String start, String end){
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String,String>>();
		try (PreparedStatement stmt = this.database.prepareStatement("EXEC Application.GetAppGrowth ?,?")) {
			stmt.setString(1, start);
			stmt.setString(2, end);
			ResultSet rs = stmt.executeQuery();
			int i = 0;
			int columns = rs.getMetaData().getColumnCount();
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

    public String CreateUserOrGetKey(String email, String firstName, String lastName, String profilePhoto) throws Exception {
			PreparedStatement createUser = this.database.prepareStatement("EXEC Application.CreateNewDefaultOrgUser ?,?,?");
			PreparedStatement getUserApiKey = this.database.prepareStatement("EXEC Application.GetApiKey ?,?,?");
			String apiKey;

			getUserApiKey.setString(1, email);
			getUserApiKey.setString(2, firstName);
			getUserApiKey.setString(3, lastName);
			ResultSet getRS = getUserApiKey.executeQuery();
			
			boolean gotKey = getRS.next();
			if (gotKey) {
				apiKey = getRS.getString("ApiKey");
			} else {
				createUser.setString(1, email);
				createUser.setString(2, firstName);
				createUser.setString(3, lastName);
				createUser.executeQuery();
				ResultSet elseGetRS = getUserApiKey.executeQuery();
				elseGetRS.next();
				apiKey = elseGetRS.getString(0);
			}
			
			return apiKey;
    }

    public int GetUserId(String apiKey) {
		try {
			PreparedStatement userIdStatement = this.database.prepareStatement("EXEC Application.GetUserIdFromAPIKey 0x"+apiKey);
			ResultSet rs = userIdStatement.executeQuery();
			rs.next();
			return rs.getInt("UserId");
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return -1;
		}
    }

	public ArrayList<Hashtable<String, String>> GetNewDirectMessages(String sinceDateTime, int currentUser, int otherUser) {
		return this.sendQuery(String.format("EXEC Application.GetNewDirectMessages \'%1$s\', %2$d, %3$d", sinceDateTime, currentUser, otherUser));
	}

	public ArrayList<Hashtable<String, String>> GetNewChannelMessages(String sinceDateTime, int channelId) {
		return this.sendQuery(String.format("EXEC Application.GetNewChannelMessages \'%1$s\', %2$d", sinceDateTime, channelId));
	}

	
	/**
	 * Calls the given stored procedure on the database with the given arguments. If
	 * the call is a query the results of the query are returned
	 * as an ArrayList of Hashtables with String-String key-value pairs, and an
	 * empty ArrayList if there are no results for the query.
	 * An empty ArrayList is also returned if the call is to a stored procedure that
	 * does not return results.
	 * 
	 * @param call   String of the call to make to the database
	 * @param argNum The number of arguments to pass to the stored procedure
	 * @param args   The arguments to pass to the stored procedure
	 * @param query  Whether the stored given stored procedure will return query
	 *               results
	 * @return
	 */
	public ArrayList<Hashtable<String, String>> callQueryProcedure(String call, int argNum, Object[] args) {
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String, String>>();
		try (CallableStatement cs = database.prepareCall(call)) {
			for (int i = 0; i < argNum; i++) {
				cs.setObject(i + 1, args[i]);
			}
			ResultSet rs = cs.executeQuery();
			if (rs.isBeforeFirst()) {
				int columns = rs.getMetaData().getColumnCount();
				while (rs.next()) {
					Hashtable<String, String> h = new Hashtable<String, String>();
					results.add(h);
					for (int j = 1; j <= columns; j++) {
						String columnName = rs.getMetaData().getColumnName(j);
						h.put(columnName, rs.getString(j));
					}
				}
				return results;
			}
			return results;
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
			Hashtable<String, String> h = new Hashtable<String, String>();
			h.put("Error", error);
			results.add(h);
			return results;
		}
	}


}
