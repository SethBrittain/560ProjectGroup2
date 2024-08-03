using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidgin.Model;
using Pidgin.Repository;

namespace Pidgin.Controller;

[ApiController]
[Route("api/DirectMessage")]
public class DirectMessageController : ControllerBase
{
	private readonly IObjectRepository<DirectMessage> _directMessageRepository;

	public DirectMessageController(IObjectRepository<DirectMessage> directMessageRepository)
	{
		_directMessageRepository = directMessageRepository;
	}

	[HttpGet("{id}"), Produces(MediaTypeNames.Application.Json)]
	public async Task<IActionResult> Get([FromRoute] int id)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));

		DirectMessage dm;
		try { dm = await _directMessageRepository.Get(id, uid); }
		catch { return NotFound(); }
		return Ok(dm);
	}

	
	[HttpGet(""), Produces(MediaTypeNames.Application.Json)]
	public async Task<IActionResult> GetAll
	(
		[FromQuery] int? senderId=null,
		[FromQuery] string? query=null,
		[FromQuery] int? recipientId=null,
		[FromQuery] int? dmId=null,
		[FromQuery] string? message=null,
		[FromQuery] DateTime? startDate=null,
		[FromQuery] DateTime? endDate=null,
		[FromQuery] int? limit=null,
		[FromQuery] int? offset=null
	)
    {
		int uid;
		if (!int.TryParse(HttpContext.User.FindFirstValue("uid"), out uid)) 
			throw new Exception("Invalid user id");
		
        IEnumerable<DirectMessage> c;
		try { c = await _directMessageRepository.GetAll(uid); }
		catch (Exception e) { Console.WriteLine(e.Message); return NotFound(); }

		if (dmId != null)
			c = c.Where(x => 
				(x.recipient.id == dmId && x.sender.id == uid) ||
				(x.sender.id == dmId && x.recipient.id == uid)
			);
		if (query != null)
			c = c.Where(x => x.message.ToLower().Contains(query.ToLower()));
		if (senderId != null)
			c = c.Where(x => x.sender.id == senderId);
		if (recipientId != null)
			c = c.Where(x => x.recipient.id == recipientId);
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

	[HttpPatch("{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Update(
		[FromRoute] int id,
		[FromForm] string message
	)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		DirectMessage cm = await _directMessageRepository.Get(id, uid);
		if (cm == null || message.Length == 0 || message.Length > 1024*16 || cm.sender.id != uid)
			return Forbid();
		cm.message = message;

		try { await _directMessageRepository.Update(cm, uid); }
		catch { return Forbid(); }
		
		return Ok();
	}

	[HttpDelete("{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> Delete([FromRoute] int id)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		
		try { await _directMessageRepository.Delete(id, uid); }
		catch { return NotFound(); }
		return Ok();
	}
}