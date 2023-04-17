package com.TeamTwo.DatabaseProject.modules.user.database;

import java.sql.Array;
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

	public ArrayList<String> CollinTestQuery()
	{
		String queryString = """
			SELECT T.FirstName, T.Email
			FROM
			(
				VALUES (1, N'Collin', N'Hammond', N'cohammo@ksu.edu')
			) T(PersonId, FirstName, LastName, Email);
		""";
		ArrayList<String> results = null;
		try (PreparedStatement stmt = this.database.prepareStatement(queryString))
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
}
