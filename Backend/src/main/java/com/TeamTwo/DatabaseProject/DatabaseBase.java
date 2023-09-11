package com.TeamTwo.DatabaseProject;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Hashtable;

public abstract class DatabaseBase {
    protected Connection connection;    

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
	protected ArrayList<Hashtable<String, String>> sendQuery(String query) {
		ArrayList<Hashtable<String, String>> results = new ArrayList<Hashtable<String, String>>();
		int i = 0;
		try (PreparedStatement stmt = this.connection.prepareStatement(query)) {
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
		try (CallableStatement cs = connection.prepareCall(call)) {
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
