package com.TeamTwo.DatabaseProject;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public abstract class ControllerBase<T extends DatabaseBase> {
    protected T database;

    protected int GetUserId() {
		try {
			PreparedStatement userIdStatement = this.database.connection.prepareStatement("EXEC Application.GetUserIdFromAPIKey 0x");
			ResultSet rs = userIdStatement.executeQuery();
			rs.next();
			return rs.getInt("UserId");
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return -1;
		}
    }
}
