package com.TeamTwo.DatabaseProject.modules.user.database;

import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Statement;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class UserDatabase {

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
}
