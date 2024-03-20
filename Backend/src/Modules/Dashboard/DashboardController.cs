using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

namespace Pidgin;

[ApiController]
[Route("/api/")]
public class DashboardController : Controller
{
	private readonly IDashboardService _dashboardService;

	public DashboardController(IDashboardService dashboardService)
	{
		_dashboardService = dashboardService;
	}

	[HttpPost("OrganizationsData")]
	public async Task<IActionResult> GetOrganizationData([FromForm] string startDate, [FromForm] string endDate)
	{
		DateTime start, end;
		IFormatProvider format = new DateTimeFormat("YYYY-MM-DD").FormatProvider;

		try
		{
			start = DateTime.Parse(startDate, format);
			end = DateTime.Parse(endDate, format);
			return Ok(await _dashboardService.GetOrganizationData(start, end));
		}
		catch (FormatException)
		{
			return BadRequest("Please Provide Dates in the format YYYY-MM-DD");
		}
	}

	[HttpPost("GetMonthlyTraffic")]
	public async Task<IActionResult> GetMonthlyTraffic([FromForm] string startDate, [FromForm] string endDate)
	{
		DateTime start, end;
		IFormatProvider format = new DateTimeFormat("YYYY-MM-DD").FormatProvider;

		try
		{
			start = DateTime.Parse(startDate, format);
			end = DateTime.Parse(endDate, format);
			List<object> result = await _dashboardService.GetMonthlyTraffic(start, end);
			return Ok(result);
		}
		catch (FormatException)
		{
			return BadRequest("Please Provide Dates in the format YYYY-MM-DD");
		}
	}

	[HttpPost("GetAppGrowth")]
	public async Task<IActionResult> GetAppGrowth()
	{
		IFormatProvider format = new DateTimeFormat("YYYY-MM-DD").FormatProvider;

		try
		{
			object result = await _dashboardService.GetAppGrowth();
			return Ok(result);
		}
		catch (FormatException)
		{
			return BadRequest("Please Provide Dates in the format YYYY-MM-DD");
		}
	}

	[HttpPost("GetGroupActivity")]
	public async Task<IActionResult> GetGroupActivity([FromForm] int organizationId, [FromForm] string startDate, [FromForm] string endDate)
	{
		DateTime start, end;
		IFormatProvider format = new DateTimeFormat("YYYY-MM-DD").FormatProvider;

		try
		{
			start = DateTime.Parse(startDate, format);
			end = DateTime.Parse(endDate, format);
			List<object> result = await _dashboardService.GetGroupActivity(organizationId, start, end);
			return Ok(result);
		}
		catch (FormatException)
		{
			return BadRequest("Please Provide Dates in the format YYYY-MM-DD");
		}
	}
}