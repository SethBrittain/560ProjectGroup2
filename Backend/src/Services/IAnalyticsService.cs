namespace Pidgin.Services;

public interface IAnalyticsService {
	public Task<object> GetOrganizationData(int id, DateTime start, DateTime end);
	public Task<List<object>> GetMonthlyTraffic(DateTime start, DateTime end);
	public Task<object> GetAppGrowth();
	public Task<List<object>> GetGroupActivity(int organizationId, DateTime start, DateTime end);
}