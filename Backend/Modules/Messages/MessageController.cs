using System.Net.Mime;
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
	
	[HttpPost("GetMessages")]
	public async Task<IActionResult> GetMessages(int channelId)
	{
		throw new NotImplementedException();
	}
}