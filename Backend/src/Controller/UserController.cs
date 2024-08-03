using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidgin.Model;
using Pidgin.Repository;

namespace Pidgin.Controller;

[ApiController]
[Route("api/User")]
public class UserController : ControllerBase
{
	private readonly IObjectRepository<User> _userRepository;

    public UserController(IObjectRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

	[HttpPost(""), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> GetCurrent()
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		User u;
		try { u = await _userRepository.Get(uid, uid); }
		catch { await HttpContext.SignOutAsync(); return NotFound(); }
		return Ok(u);
	}
	
    [HttpGet("{id}"), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		User u;
		try { u = await _userRepository.Get(id, uid); }
		catch { return NotFound(); }
        return Ok(u);
    }

	[HttpGet(""), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Get
	(
		[FromQuery] int? organizationId=null,
		[FromQuery] string? query=null,
		[FromQuery] string? email=null,
		[FromQuery] string? firstName=null,
		[FromQuery] string? lastName=null,
		[FromQuery] string? title=null,
		[FromQuery] int? limit=null,
		[FromQuery] int? offset=null
	)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));

		IEnumerable<User> users;
		try { users = await _userRepository.GetAll(uid); }
		catch { return NotFound(); }

		if (query != null)
		{
			users = users.Where(x => 
				x.email.Contains(query, StringComparison.CurrentCultureIgnoreCase) || 
				x.firstName.Contains(query, StringComparison.CurrentCultureIgnoreCase) || 
				x.lastName.Contains(query, StringComparison.CurrentCultureIgnoreCase) || 
				x.title == null ? false : x.title.Contains(query, StringComparison.CurrentCultureIgnoreCase)
			);
		}
		if (organizationId != null)
			users = users.Where(x => x.organizationId == organizationId);
		if (email != null)
			users = users.Where(x => x.email.Contains(email));
		if (firstName != null)
			users = users.Where(x => x.firstName.Contains(firstName));
		if (lastName != null)
			users = users.Where(x => x.lastName.Contains(lastName));
		if (title != null)
			users = users.Where(x => x.title.Contains(title));
		if (limit != null)
			users = users.Take(limit.Value);

		if (!users.Any())
			return NotFound();

		return Ok(users);
	}

	[HttpDelete("{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Delete(int id)
	{
		return Forbid();
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));

		try { await _userRepository.Delete(id, uid); }
		catch { return NotFound(); }
		return Ok();
	}

	[HttpPatch("")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Update
	(
		[FromBody] User u
	)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));

		if (u.id != uid)
			return NotFound();

		try { await _userRepository.Update(u, uid); }
		catch (Exception e) { Console.WriteLine(e.Message); return NotFound(); }

		return Ok();
	}
}