package com.TeamTwo.DatabaseProject.modules.analytics;

import java.util.ArrayList;
import java.util.Hashtable;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import com.TeamTwo.DatabaseProject.ControllerBase;

public class AnalyticsController extends ControllerBase<AnalyticsDatabase> {

    @Autowired
    public AnalyticsController(AnalyticsDatabase database)
    {
        this.database = database;
    }
    /**
	 * Gets data about all organizations in the database
	 * 
	 * @return ArrayList<Object> - String OrgName, int ActiveUserCount, int MessageCount
	 */
	@PostMapping("/api/OrganizationsData")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetOrganizationData(@RequestParam String startDate,	@RequestParam String endDate) {
		return database.GetOrganizationData(startDate, endDate);
	}

	@PostMapping("/api/GetMonthlyTraffic")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetMonthlyTraffic(@RequestParam String startDate,
			@RequestParam String endDate) {
		return database.GetMonthlyTraffic(startDate, endDate);
	}

	@PostMapping("/api/GetAppGrowth")
	@ResponseBody
	public ArrayList<Hashtable<String,String>> GetAppGrowth(@RequestParam String startDate, @RequestParam String endDate){
		return this.database.GetAppGrowth(startDate, endDate);	
	}

	@PostMapping("/api/GetGroupActivity")
	@ResponseBody
	public ArrayList<Hashtable<String, String>> GetGroupActivity(@RequestParam int organizationId, @RequestParam String startDate, @RequestParam String endDate) {
		String call = "{call Application.GetGroupActivity(?,?,?)}";
		Object[] args = { organizationId, startDate, endDate };
		return this.database.callQueryProcedure(call, 3, args);
	}
}
