namespace Pidgin;

public interface IDashboardService
{
	Task<object> GetOrganizationData(DateTime start, DateTime end);
	Task<List<object>> GetMonthlyTraffic(DateTime start, DateTime end);
	Task<object> GetAppGrowth();
	Task<List<object>> GetGroupActivity(int organizationId, DateTime start, DateTime end);
}