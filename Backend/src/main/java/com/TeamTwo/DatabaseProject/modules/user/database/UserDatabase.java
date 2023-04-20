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
		ArrayList<String> results = null;
		try (PreparedStatement stmt = this.database.prepareStatement(query))
		{
			ResultSet rs = stmt.executeQuery();
			if (rs.isBeforeFirst())
			{
				results = new ArrayList<String>();
				results.add("true");
			}
			while (rs.next())
			{
				results.add(rs.getString(1));
				results.add(rs.getString(2));
			}
		}	catch (SQLException e) {
			String state = e.getSQLState();
			System.out.println(e.toString());
			results.add("false");
			results.add(state);
		}
		return results;
	}

	/**
	 * Gets data about all organizations in the database, counts messages for MessageCount from start to end
	 * @return ArrayList - String OrgName, int ActiveUserCount, int MessageCount
	 */
	public ArrayList<String> GetOrgsData(DateTimeOffset start, DateTimeOffset end)
	{
		String query = "EXEC Application.GetOrganizationData";
		ArrayList<String> results = null;
		try (PreparedStatement stmt = this.database.prepareStatement(query))
		{
			ResultSet rs = stmt.executeQuery();
			if (rs.isBeforeFirst())
			{
				results = new ArrayList<String>();
				results.add("Good");
			}
			while (rs.next())
			{
				results.add(rs.getString(1));
				results.add(rs.getString(2));
				results.add(rs.getString(3));
			}
		}
		catch (SQLException e) {
			String state = e.getSQLState();
			System.out.println(e.toString());
			results = new ArrayList<String>();
			results.add("Error");
			results.add(state);
		}
		return results;
	}

	/**
	 * Gets all the messages from the given channel
	 * @param ChannelId The ID number of the channel to get messages from
	 * @return ArrayList - Message
	 */
	public ArrayList<String> GetChannelMessages(int ChannelId)
	{
		String query = "EXEC Application.GetAllChannelMessages " + ChannelId;
		ArrayList<String> results = null;
		try (PreparedStatement stmt = this.database.prepareStatement(query))
		{
			ResultSet rs = stmt.executeQuery();
			if (rs.isBeforeFirst()) results = new ArrayList<String>();
			results.add("Good");
			while (rs.next())
			{
				results.add(rs.getString(1));
			}
		}
		catch (SQLException e) {
			String state = e.getSQLState();
			System.out.println(e.toString());
			results = new ArrayList<String>();
			results.add("Error");
			results.add(state);
		}
		return results;
	}

	/**
	 * Gets all direct messages between the two users with the given userIDs
	 * @param userA The ID of the first user
	 * @param userB The ID of the second user
	 * @return ArrayList - Message
	 */
	public ArrayList<String> GetDirectMessages(int userA, int userB)
	{
		String query = "EXEC Application.GetAllMessagesBetweenUsers " + userIDA + " " + userIDB;
		ArrayList<String> results = null;
		try (PreparedStatement stmt = this.database.prepareStatement(query))
		{
			ResultSet rs = stmt.executeQuery();
			if (rs.isBeforeFirst()) results = new ArrayList<String>();
			results.add("Good");
			while (rs.next())
			{
				results.add(rs.getString(1));
			}
		}
		catch (SQLException e) {
			String state = e.getSQLState();
			System.out.println(e.toString());
			results = new ArrayList<String>();
			results.add("Error");
			results.add(state);
		}
		return results;
	}

	/**
	 * Gets all the group that are in the given organization
	 * @param org The organization to get groups of
	 * @return ArrayList<String> of the groups in the given organization
	 */
	public ArrayList<String> GetGroups(String org)
	{
		String query = "EXEC Application.GetGroups " + org;
		ArrayList<String> results = null;
		try (PreparedStatement stmt = this.database.prepareStatement(query))
		{
			ResultSet rs = stmt.executeQuery();
			if (rs.isBeforeFirst()) results = new ArrayList<String>();
			while (rs.next())
			{
				results.add(rs.getString(1));
				//TODO: add more info from query to results
			}
		}
		catch (SQLException e) {
			System.out.println(e.toString());
		}
		return results;
	}

	/**
	 * Gets all the channels in a given organization and group
	 * @param org The organization to get the group and channels from
	 * @param group The group to get channels from
	 */
	public ArrayList<String> GetChannels(String org, String group)
	{
		String query = "EXEC Application.GetChannels " + org + " " + group;
		ArrayList<String> results = null;
		try (PreparedStatement stmt = this.database.prepareStatement(query))
		{
			ResultSet rs = stmt.executeQuery();
			if (rs.isBeforeFirst()) results = new ArrayList<String>();
			while (rs.next())
			{
				results.add(rs.getString(1));
				//TODO: add more info from query to results
			}
		}
		catch (SQLException e) {
			System.out.println(e.toString());
		}
		return results;
	}

}
