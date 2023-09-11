package com.TeamTwo.DatabaseProject.modules.messaging;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Hashtable;

import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.TeamTwo.DatabaseProject.DatabaseBase;

@Service
public class MessagingDatabase extends DatabaseBase {

	@Autowired
	public MessagingDatabase(Connection db) {
		this.connection = db;
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

	public ArrayList<Hashtable<String, String>> GetNewDirectMessages(String sinceDateTime, int currentUser, int otherUser) {
		return this.sendQuery(String.format("EXEC Application.GetNewDirectMessages \'%1$s\', %2$d, %3$d", sinceDateTime, currentUser, otherUser));
	}

	public ArrayList<Hashtable<String, String>> GetNewChannelMessages(String sinceDateTime, int channelId) {
		return this.sendQuery(String.format("EXEC Application.GetNewChannelMessages \'%1$s\', %2$d", sinceDateTime, channelId));
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
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.UpdateMessage ?,?")) {
			stmt.setInt(1, msgId);
			stmt.setString(2, message);
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
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.InsertDirectMessage ?,?,?")) {
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
	 * Deletes the message with the given messageId from the database
	 * 
	 * @param msgId The messageId to delete
	 * @return Boolean - true if the insertion is successful, false otherwise
	 */
	public Boolean DeleteMessage(int msgId) {
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.DeleteMessage ?")) { // InsertNewUser
			stmt.setInt(1, msgId);
			return stmt.execute();
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
		}
		return false;
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
			PreparedStatement stmt = this.connection.prepareStatement(insert, PreparedStatement.RETURN_GENERATED_KEYS);
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

	/*
	 * Gets all the messages from the given channel
	 * 
	 * @param ChannelId The ID number of the channel to get messages from
	 * 
	 * @return ArrayList->Hashtables - MsgId, Message, UpdatedOn, SenderId,
	 * FirstName, LastName, ProfilePhoto
	 */
	public ArrayList<Hashtable<String, String>> GetAllChannelMessages(int userId, int channelId) {
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
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.SearchUserMessages ?,?")) {
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
	 * Gets the name of the channel with the given channelId
	 * 
	 * @param channelId The channelId of the channel to get the name of
	 * @return ArrayList->Hashtable - Name
	 */
	public ArrayList<Hashtable<String, String>> GetChannelName(int channelId) {
		String query = "EXEC Application.GetChannelName " + channelId;
		return sendQuery(query);
	}
}