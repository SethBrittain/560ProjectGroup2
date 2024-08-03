using System.Runtime.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using Pidgin.Exceptions;
using Pidgin.Services;

namespace Pidgin;

[ApiController]
[Route("api/Analytics")]
public class AnalyticsController : ControllerBase
{
	private readonly IAnalyticsService _dashboardService;

	public AnalyticsController(IAnalyticsService dashboardService)
	{
		_dashboardService = dashboardService;
	}

	[HttpGet("OrganizationsData")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> GetOrganizationData([FromQuery] string startDate, [FromQuery] string endDate)
	{
		DateTime start, end;
		IFormatProvider format = new DateTimeFormat("YYYY-MM-DD").FormatProvider;

		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));

		try
		{
			start = DateTime.Parse(startDate, format);
			end = DateTime.Parse(endDate, format);
			return Ok(await _dashboardService.GetOrganizationData(uid, start, end));
		}
		catch (FormatException)
		{
			return BadRequest("Please Provide Dates in the format YYYY-MM-DD");
		}
		catch (DataNotFoundException)
		{
			return NotFound("No Data Found");
		}
	}

	[HttpGet("MonthlyTraffic")]
	public async Task<IActionResult> GetMonthlyTraffic([FromQuery] string startDate, [FromQuery] string endDate)
	{
		object[] res = { new {
			Month= "January",
			Year= 2021,
			MessagesSent= 100,
			Rank= 1
		},new {
			Month= "February",
			Year= 2021,
			MessagesSent= 100,
			Rank= 2
		}
		};
		return Ok(res);
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

	[HttpGet("AppGrowth")]
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

	[HttpGet("GroupActivity")]
	public async Task<IActionResult> GetGroupActivity([FromQuery] int orgId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		try
		{
			List<object> result = await _dashboardService.GetGroupActivity(orgId, startDate, endDate);
			return Ok(result);
		}
		catch (FormatException)
		{
			return BadRequest("Please Provide Dates in the format YYYY-MM-DD");
		}
	}
}