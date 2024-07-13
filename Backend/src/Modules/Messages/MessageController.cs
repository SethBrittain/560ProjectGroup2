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
        string? uidClaim = HttpContext.User.FindFirstValue("uid");
        if (!int.TryParse(uidClaim, out int uid))
            return Unauthorized("User id claim is not a valid integer");

        List<object> messages = await _messageService.GetAllChannelMessages(channelId, uid);
        return Ok(messages);
	}
}