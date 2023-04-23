package com.TeamTwo.DatabaseProject.modules.user.database;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.sql.PreparedStatement;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import microsoft.sql.DateTimeOffset;

@Service
public class UserDatabase 
{

    private Connection database;

    @Autowired
    public UserDatabase(Connection db)
    {
        this.database = db;
    }

	/**
	 * Helper method to send a query to the database and parse the results for the given query
	 * @param columns The number of columns the query will return
	 * @param query The query to send to the database
	 * @return An ArrayList with the results of the query: "Empty" will be in the first index if the results were empty
	 */
	private ArrayList<String> sendQuery(int columns, String query)
	{
		ArrayList<String> results = new ArrayList<String>();
		try (PreparedStatement stmt = this.database.prepareStatement(query))
		{
			ResultSet rs = stmt.executeQuery();
			if (rs.isBeforeFirst())
			{
				results = new ArrayList<String>();
				results.add("No Error");
			}
			else results.add("Empty");
			while (rs.next())
			{
				for (int i = 1; i <= columns; i++)
					results.add(rs.getString(i));
			}
		}	
		catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
			results.add("Error");
			results.add(error);
		}
		return results;
	}

	/**
	 * Example of getting info from a database for reference
	 * @return ArrayList with: FirstName, Email
	 */
	public ArrayList<String> TestQuery()
	{
		String query = """
			SELECT T.FirstName, T.Email
			FROM
			(
				VALUES (1, N'Joe', N'Cool', N'joecool@ksu.edu')
			) T(PersonId, FirstName, LastName, Email);
		""";
		return sendQuery(2, query);
	}

	/**
	 * Gets data about all organizations in the database, counts messages for MessageCount from start to end
	 * @return ArrayList - String OrgName, int ActiveUserCount, int MessageCount
	 */
	public ArrayList<String> GetOrganizationData(DateTimeOffset start, DateTimeOffset end)
	{
		String query = "EXEC Application.GetOrganizationData " + start + " " + end;
		return sendQuery(3, query);
	}

	/**
	 * Gets all the messages from the given channel
	 * @param ChannelId The ID number of the channel to get messages from
	 * @return ArrayList - Message, null if no results
	 */
	public ArrayList<String> GetAllChannelMessages(int ChannelId)
	{
		String query = "EXEC Application.GetAllChannelMessages " + ChannelId;
		return sendQuery(1, query);
	}

	/**
	 * Gets all direct messages between the two users with the given userIDs
	 * @param userA The ID of the first user
	 * @param userB The ID of the second user
	 * @return ArrayList - Message, SenderId
	 */
	public ArrayList<String> GetDirectMessages(int userA, int userB)
	{
		String query = "EXEC Application.GetDirectMessages " + userA + " " + userB;
		return sendQuery(2, query);
	}


	/**
	 * Get all messages that match a substring within a given channel
	 * @param substring substring to match with
	 * @param channelId channel to search in
	 * @return ArrayList - Message, SenderId
	 */
	public ArrayList<String> GetAllMessagesMatchingSubstring(String substring, int channelId)
	{
		String query = "EXEC Application.GetAllMessagesMatchingSubstring " + substring + " " + channelId;
		return sendQuery(2, query);		
	}

	/**
	 * Get all channels in an Organization
	 * @param organizationId organization identification
	 * @return ArrayList - ChannelId, ChannelName
	 */
	public ArrayList<String> GetAllChannelsInOrganization(int organizationId)
	{
		String query = "EXEC Application.GetAllChannelsInOrganization " + organizationId;
		return sendQuery(2, query);		
	}

	/**
	 * Get all users in an organization 
	 * @param organizationId org identification
	 * @return ArrayList - UserId, FirstName, LastName, Username
	 */
	public ArrayList<String> GetAllUsersInOrganization(int organizationId)
	{
		String query = "EXEC Application.GetAllUsersInOrganization " + organizationId;
		return sendQuery(4, query);
	}

	/**
	 * Get user info via username 
	 * @param email email 
	 * @return ArrayList - Username, FirstName, LastName, Password, OrganizationId
	 */
	public ArrayList<String> GetUserInfo(String email)
	{
		String query = "EXEC Application.GetUserInfo " + email;
		return sendQuery(5, query);
	}

	/**
	 * Insert Message into channel
	 * @param message Message to insert
	 * @param senderId sender id
	 * @param channelId recipient id
	 */
	public void InsertMessageIntoChannel(String message, int senderId, int recipientId)
	{
		String query = "EXEC Application.InsertMessageInto " + message + " " + senderId + " " + channelId; 
	}

	/**
	 * Insert direct message
	 * @param message message to insert
	 * @param senderId sender id
	 * @param recipientId recipient id 
	 */
	public void InsertDirectMessage(String message, int senderId, int recipientId)
	{
		String query = "EXEC Application.InsertMessageInto " + message + " " + senderId + " " + RecipientId; 
	}

	




	

}
