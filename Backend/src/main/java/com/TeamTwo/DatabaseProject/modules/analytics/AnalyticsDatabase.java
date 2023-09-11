package com.TeamTwo.DatabaseProject.modules.analytics;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Hashtable;

import com.TeamTwo.DatabaseProject.DatabaseBase;

public class AnalyticsDatabase extends DatabaseBase {
    /**
	 * Gets data about all organizations in the database, counts messages for
	 * MessageCount from start to end
	 * 
	 * @return ArrayList - String OrgName, int ActiveUserCount, int MessageCount
	 */
	public ArrayList<Hashtable<String, String>> GetOrganizationData(String start, String end) {
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String,String>>();
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.GetOrganizationData ?,?")) {
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
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.GetMonthlyTraffic ?,?")) {
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
		try (PreparedStatement stmt = this.connection.prepareStatement("EXEC Application.GetAppGrowth ?,?")) {
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

    
}
