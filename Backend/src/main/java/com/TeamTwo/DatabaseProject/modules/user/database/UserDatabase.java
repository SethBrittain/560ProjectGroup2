package com.TeamTwo.DatabaseProject.modules.user.database;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.PreparedStatement;
import java.util.ArrayList;
import java.util.Hashtable;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.TeamTwo.DatabaseProject.exceptions.ApiException;

@Service
public class UserDatabase {

	private Connection database;

	@Autowired
	public UserDatabase(Connection db) {
		this.database = db;
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
		try (CallableStatement cs = database.prepareCall(call)) {
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

	public Boolean callStatementProcedure(String call, int argNum, Object[] args) {
		try (CallableStatement cs = database.prepareCall(call)) {
			for (int i = 0; i < argNum; i++) {
				cs.setObject(i + 1, args[i]);
			}
			cs.execute();
			return true;
		} catch (SQLException e) {
			String error = e.toString();
			System.out.println(error);
			return false;
		}
	}

	public String CreateUserOrGetKey(String email, String firstName, String lastName, String profilePhoto) {
		try (PreparedStatement createUser = this.database
				.prepareStatement("EXEC Application.CreateNewDefaultOrgUser ?,?,?,?")) {
			PreparedStatement getUserApiKey = this.database.prepareStatement("EXEC Application.GetApiKey ?,?,?");
			String apiKey;

			getUserApiKey.setString(1, email);
			getUserApiKey.setString(2, firstName);
			getUserApiKey.setString(3, lastName);
			getUserApiKey.setString(4, profilePhoto);
			ResultSet getRS = getUserApiKey.executeQuery();

			if (getRS.isBeforeFirst()) {
				getRS.next();
				apiKey = getRS.getString("ApiKey");
			} else {
				createUser.setString(1, email);
				createUser.setString(2, firstName);
				createUser.setString(3, lastName);
				createUser.executeQuery();
				ResultSet elseGetRS = getUserApiKey.executeQuery();
				elseGetRS.next();
				apiKey = elseGetRS.getString(0);
			}

			return apiKey;
		} catch (Exception e) {
			System.out.println(e.getMessage());
			throw new RuntimeException();
		}
	}

	public int GetUserId(String apiKey) {
		try (PreparedStatement userIdStatement = database
				.prepareStatement(String.format("EXEC Application.GetUserIdFromAPIKey0x%s", apiKey))) {
			ResultSet rs = userIdStatement.executeQuery();
			return rs.getInt("UserId");
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return -1;
		}
	}
}