using System.CodeDom.Compiler;
using System.Net.Mime;
using System.Net.WebSockets;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using pidgin.models;
using pidgin.services;

namespace pidgin.Controllers;

public class MessageWebsocketController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageWebsocketController(IMessageService messageService)
    {
        this._messageService = messageService;
    }
	
	[HttpGet("/ws")]
	public async Task Connect()
	{
		if (HttpContext.WebSockets.IsWebSocketRequest)
		{
			using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
			await Echo(webSocket);
		}
		else
		{
			HttpContext.Response.StatusCode = 400;
		}
	}

	private static async Task Echo(WebSocket ws)
	{
		var buffer = new byte[1024 * 4];
		var receiveResult = await ws.ReceiveAsync(
			new ArraySegment<byte>(buffer), 
			CancellationToken.None
		);

		while (!receiveResult.CloseStatus.HasValue)
		{
			await ws.SendAsync(
				new ArraySegment<byte>(buffer, 0, receiveResult.Count),
				receiveResult.MessageType,
				receiveResult.EndOfMessage,
				CancellationToken.None
			);

			receiveResult = await ws.ReceiveAsync(
				new ArraySegment<byte>(buffer),
				CancellationToken.None
			);
		}
	}
}