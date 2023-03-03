package com.TeamTwo.DatabaseProject;

import java.sql.Connection;
import java.sql.SQLException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;

import com.microsoft.sqlserver.jdbc.SQLServerDataSource;

@Configuration
public class DatabaseConfiguration {
    @Autowired
    private Environment env;

    @Bean(name = "DatabaseConnectionObject")
    public Connection connection() throws SQLException
    {
        SQLServerDataSource ds = new SQLServerDataSource();
        ds.setUser(env.getProperty("database.user"));
        ds.setPassword(env.getProperty("database.password"));
        ds.setServerName(env.getProperty("database.address"));
        ds.setPortNumber(Integer.parseInt(env.getProperty("database.port")));
        ds.setDatabaseName(env.getProperty("database.name"));
        ds.setEncrypt(false);
        Connection connectionObject = ds.getConnection();
        return connectionObject;
    }

}
