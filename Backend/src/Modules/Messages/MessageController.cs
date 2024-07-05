using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using pidgin.models;
using pidgin.services;

namespace pidgin.Controllers;

[ApiController]
[Route("api")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        this._messageService = messageService;
    }
	
	[HttpPost("GetAllChannelMessages")]
	public async Task<IActionResult> GetAllChannelMessages([FromForm] int channelId)
    {
        ClaimsPrincipal u = HttpContext.User;
        int uid = int.Parse(u.FindFirstValue("uid"));
        List<object> messages = await _messageService.GetAllChannelMessages(channelId, uid);
        return Ok(messages);
	}
}