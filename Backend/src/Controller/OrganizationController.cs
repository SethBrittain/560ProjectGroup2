using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidgin.Model;
using Pidgin.Repository;

namespace Pidgin.Controller;

[ApiController]
[Route("api/Organization")]
public class OrganizationController : ControllerBase
{
    private readonly IObjectRepository<Organization> _organizationRepository;

    public OrganizationController(IObjectRepository<Organization> organizationRepository)
    {
		_organizationRepository = organizationRepository;
    }
	
	[HttpGet("{id}"), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> Get(int id)
    {
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		Organization u;
		try { u = await _organizationRepository.Get(id, uid); }
		catch { return NotFound(); }
        return Ok(u);
    }

	[HttpGet(""), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Get
	(
		[FromQuery] string? name=null,
		[FromQuery] DateTime? startDate=null,
		[FromQuery] DateTime? endDate=null
	)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));

		IEnumerable<Organization> users;
		try { users = await _organizationRepository.GetAll(uid); }
		catch { return NotFound(); }
		
		if (name != null)
			users = users.Where(x => x.name.Contains(name));
		if (startDate != null)
			users = users.Where(x => x.createdOn >= startDate);
		if (endDate != null)
			users = users.Where(x => x.createdOn <= endDate);

		if (!users.Any())
			return NotFound();

		return Ok(users);
	}

	[HttpDelete("{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Delete(int id)
	{
		return Forbid();
	}

	[HttpPut("")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Put()
	{
		return Forbid();
	}
}