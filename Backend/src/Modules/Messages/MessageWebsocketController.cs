using System.CodeDom.Compiler;
using System.Net;
using System.Net.Mime;
using System.Net.WebSockets;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pidgin.models;
using pidgin.services;
using Pidgin.Services;

namespace pidgin.Controllers;

[Route("/ws")]
[Controller]
public class MessageWebsocketController : ControllerBase
{
    private readonly IMessageService _messageService;
	private IWebSocketService webSocketService;

    public MessageWebsocketController(IMessageService messageService, IWebSocketService webSocketService)
    {
        this._messageService = messageService;
		this.webSocketService = webSocketService;
    }
	
	[HttpGet("channel")]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> HandleChannelConnection(int id)
    {
        int uid = int.Parse(HttpContext.User.FindFirst("uid")?.Value ?? "-1");
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            Console.WriteLine("Incoming websocket connection");
            Console.WriteLine($"User {uid} connected to channel {id}");

            IPAddress? ip = HttpContext.Connection.RemoteIpAddress;
            if (ip != null) Console.WriteLine($"Client connected from: {ip}");

            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await this.webSocketService.HandleWebsocketConnection(webSocket);
            return new EmptyResult();
        }
        return new BadRequestResult();
    }

    [HttpGet("direct")]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> HandleDirectConnection(int id)
    {
        int uid = int.Parse(HttpContext.User.FindFirst("uid")?.Value ?? "-1");
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            Console.WriteLine("Incoming websocket connection");
            Console.WriteLine($"User {uid} connected to direct {id}");

            IPAddress? ip = HttpContext.Connection.RemoteIpAddress;
            if (ip != null) Console.WriteLine($"Client connected from: {ip}");

            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await this.webSocketService.HandleWebsocketConnection(webSocket);
            return new EmptyResult();
        }
        return new BadRequestResult();
    }
}