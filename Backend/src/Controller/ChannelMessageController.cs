using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidgin.Model;
using Pidgin.Repository;

namespace Pidgin.Controller;

[ApiController]
[Route("api/ChannelMessage")]
public class ChannelMessageController : ControllerBase 
{
	private readonly IObjectRepository<ChannelMessage> _channelMessageRepository;

    public ChannelMessageController(IObjectRepository<ChannelMessage> channelMessageRepository)
    {
		_channelMessageRepository = channelMessageRepository;
    }

    [HttpGet("{id}"), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
        ChannelMessage c;
		try { c = await _channelMessageRepository.Get(id, uid); }
		catch { return NotFound(); }
        return Ok(c);
    }

	[HttpGet(""), Produces(MediaTypeNames.Application.Json)]
	[Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> Get
	(
		[FromQuery] int? senderId=null, 
		[FromQuery] int? channelId=null,
		[FromQuery] string? message=null,
		[FromQuery] DateTime? startDate=null,
		[FromQuery] DateTime? endDate=null,
		[FromQuery] int? limit=null,
		[FromQuery] int? offset=null
	)
    {
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
        IEnumerable<ChannelMessage> c;
		try { c = await _channelMessageRepository.GetAll(uid); }
		catch { return NotFound(); }

		if (senderId != null)
			c = c.Where(x => x.sender.id == senderId);
		if (channelId != null)
			c = c.Where(x => x.channel.channelId == channelId);
		if (message != null)
			c = c.Where(x => x.message.Contains(message));
		if (startDate != null)
			c = c.Where(x => x.createdOn >= startDate);
		if (endDate != null)
			c = c.Where(x => x.createdOn <= endDate);
		if (limit != null)
		{
			if (offset != null) c = c.Skip(offset.Value);
			c = c.Take(limit.Value);
		}
		else c = c.Take(100);
		
		if (!c.Any())
			return NotFound();
		
        return Ok(c);
    }

	[HttpPut("")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Create()
	{
		return Forbid();
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		// ChannelMessage cm = new(
		// 	channelMessageId: -1,
		// 	senderId: uid,
		// 	channelId: -1,
		// 	message: "",
		// 	createdOn: DateTime.Now,
		// 	updatedOn: DateTime.Now
		// );
		
		// try { await _channelMessageRepository.Create(cm); }
		// catch { return NotFound(); }
		// return Ok();
	}

	[HttpPatch("{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Update(
		[FromRoute] int id,
		[FromForm] string message
	)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		ChannelMessage cm = await _channelMessageRepository.Get(id, uid);
		if (cm == null || message.Length == 0 || message.Length > 1024*16 || cm.sender.id!=uid)
			return Forbid();

		cm.message = message;

		try { await _channelMessageRepository.Update(cm, uid); }
		catch { return Forbid(); }
		
		return Ok();
	}

	[HttpDelete("{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Delete([FromRoute] int id)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		try { await _channelMessageRepository.Delete(id, uid); }
		catch { return NotFound(); }
		return Ok();
	}
}