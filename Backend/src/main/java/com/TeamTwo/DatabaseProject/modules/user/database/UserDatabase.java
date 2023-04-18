package com.TeamTwo.DatabaseProject.modules.user.database;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.sql.PreparedStatement;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class UserDatabase 
{

    private Connection database;

    @Autowired
    public UserDatabase(Connection db)
    {
        this.database = db;
    }

    public void testQuery()
    {
        String queryString = """
            INSERT INTO 
            test(first_name, last_name) 
            VALUES('seth', 'Brittain')
        """;
        try (Statement stmt = this.database.createStatement())
        {
            stmt.executeUpdate(queryString);
        } catch (SQLException e) {
            System.out.println(e.toString());
        }
    }

	/**
	 * Example of getting info from a database for reference
	 * @return ArrayList<String> of the results of the query that was run
	 */
	public ArrayList<String> CollinTestQuery()
	{
		String query = """
			SELECT T.FirstName, T.Email
			FROM
			(
				VALUES (1, N'Collin', N'Hammond', N'cohammo@ksu.edu')
			) T(PersonId, FirstName, LastName, Email);
		""";
		ArrayList<String> results = null;
		try (PreparedStatement stmt = this.database.prepareStatement(query))
		{
			ResultSet rs = stmt.executeQuery();
			if (rs.isBeforeFirst()) results = new ArrayList<String>();
			while (rs.next())
			{
				results.add(rs.getString(1));
				results.add(rs.getString(2));
			}
		}	catch (SQLException e) {
			System.out.println(e.toString());
		}
		return results;
	}

	/**
	 * Gets all the group that are in an organization
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

}
