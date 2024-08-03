using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidgin.Repository;
using Pidgin.Model;

namespace Pidgin.Modules.Channels;

[ApiController]
[Route("api/Channel")]
public class ChannelController : ControllerBase
{
    private readonly IObjectRepository<Channel> _channelRepository;

    public ChannelController(IObjectRepository<Channel> channelRepository)
    {
		_channelRepository = channelRepository;
    }

    [HttpGet("{id}"), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
        Channel c;
		try { c = await _channelRepository.Get(id, uid); }
		catch { return NotFound(); }
        return Ok(c);
    }

	[HttpGet(""), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> Get
	(
		[FromQuery] int? groupId=null,
		[FromQuery] string? name=null, 
		[FromQuery] DateTime? startDate=null, 
		[FromQuery] DateTime? endDate=null,
		[FromQuery] int? limit=null
	)
    {
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
        IEnumerable<Channel> c;
		try { c = await _channelRepository.GetAll(uid); }
		catch { return NotFound(); }

		if (groupId != null)
			c = c.Where(x => x.groupId == groupId);
		if (name != null)
			c = c.Where(x => x.name.Contains(name));
		if (startDate != null)
			c = c.Where(x => x.createdOn >= startDate);
		if (endDate != null)
			c = c.Where(x => x.createdOn <= endDate);
		if (limit != null)
			c = c.Take(limit.Value);
		
		if (!c.Any())
			return NotFound();
		
        return Ok(c);
    }

	[HttpDelete("{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Delete([FromRoute] int id)
	{
		return Forbid();
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		try { await _channelRepository.Delete(id, uid); }
		catch { return NotFound(); }
		return Ok();
	}

	[HttpPut("")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Create([FromBody] string name)
	{
		return Forbid();
		// int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		// Channel c = new(
		// 	channelId: -1,
		// 	groupId: -1,
		// 	name: name,
		// 	createdOn: DateTime.Now,
		// 	updatedOn: DateTime.Now
		// );

		// try { await _channelRepository.Create(c); }
		// catch { return NotFound(); }
		// return Ok();
	}
}