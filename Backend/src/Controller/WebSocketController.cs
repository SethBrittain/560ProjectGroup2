using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidgin.Model;
using Pidgin.Repository;
using Pidgin.Util;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Pidgin.Modules.Messaging;

[Controller]
[Route("chat")]
public class ChatController : ControllerBase
{
	private const int MAX_MESSAGE_SIZE = 4096 * 4;
	private static Dictionary<int, WebSocketChannel> _channels = new();
    private static Dictionary<WebSocketDirectKey, WebSocketDirectGroup> _directMessages = new();
	private readonly IObjectRepository<ChannelMessage> _channelMessageRepo;
	private readonly IObjectRepository<DirectMessage> _directRepo;
	private readonly IObjectRepository<User> _userRepo;
	private readonly IObjectRepository<Channel> _channelRepo;

	/// <summary>
	/// Constructor for WebSocketController
	/// </summary>
	/// <param name="manager">The websocket managing service</param>
	/// <param name="channelService">The channel managing service</param>
	/// <param name="messageService">The message managing service</param>
	public ChatController
	(
		IObjectRepository<ChannelMessage> channelMessageRepo, 
		IObjectRepository<DirectMessage> directRepo, 
		IObjectRepository<User> userRepo,
		IObjectRepository<Channel> channelRepo
	)
    {
		_channelMessageRepo = channelMessageRepo;
		_directRepo = directRepo;
		_userRepo = userRepo;
		_channelRepo = channelRepo;
    }


	[HttpGet("channel/{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task ChatChannel([FromRoute] int id)
	{
		if (HttpContext.WebSockets.IsWebSocketRequest)
			using (WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
			{
				int uid;
				if (!int.TryParse(HttpContext.User.FindFirstValue("uid"), out uid)) 
					throw new Exception("Invalid user id");
				User sender = await _userRepo.Get(uid, uid);

				if (!_channels.ContainsKey(id))
					_channels.Add(id, new WebSocketChannel(id, ()=>{_channels.Remove(id);}));
				await _channels[id].Join(uid, webSocket);

				byte[] buffer = Enumerable.Repeat<byte>(0, MAX_MESSAGE_SIZE).ToArray();
				while (webSocket.State != WebSocketState.Closed) try
				{
					string message = await NextMessage(webSocket, buffer);
					ChannelMessage cm = new ChannelMessage
					(
						channelMessageId: -1,
						message: message,
						createdOn: DateTime.Now,
						updatedOn: DateTime.Now,
						channel: await _channelRepo.Get(id, uid),
						sender: sender
					);
					int returnedId = await _channelMessageRepo.Create(cm);
					Console.WriteLine(returnedId);
					ChannelMessage result = await _channelMessageRepo.Get(returnedId, uid);
					await _channels[id].Broadcast(result);
				} catch (Exception e) { Console.WriteLine(e.Message); Console.WriteLine(e.StackTrace); break; }

				await _channels[id].Disconnect(uid);
			}
		else HttpContext.Response.StatusCode = 400;
	}

	[HttpGet("direct/{id}")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task ChatDirect([FromRoute] int id)
	{
		if (HttpContext.WebSockets.IsWebSocketRequest)
			using (WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
			{
				int uid;
				if (!int.TryParse(HttpContext.User.FindFirstValue("uid"), out uid)) 
					throw new Exception("Invalid user id");
				User sender = await _userRepo.Get(uid, uid);

				if (!_directMessages.ContainsKey(new WebSocketDirectKey(uid, id)))
					_directMessages.Add(
						new WebSocketDirectKey(uid, id), 
						new WebSocketDirectGroup(()=>{
							_directMessages.Remove(new WebSocketDirectKey(uid, id));
						})
					);
				await _directMessages[new WebSocketDirectKey(uid,id)].Join(uid, webSocket);

				byte[] buffer = Enumerable.Repeat<byte>(0, MAX_MESSAGE_SIZE).ToArray();
				
				while (webSocket.State != WebSocketState.Closed) try
				{
					string message = await NextMessage(webSocket, buffer);
					Console.WriteLine("DM constructor");
					DirectMessage dm = new DirectMessage(
						directMessageId: -1,
						message: message,
						createdOn: DateTime.Now,
						updatedOn: DateTime.Now,
						sender: sender,
						recipient: await _userRepo.Get(id, uid)
					);
					int returnedId = await _directRepo.Create(dm);
					Console.WriteLine("created:" + returnedId);
					DirectMessage result = await _directRepo.Get(returnedId, uid);
					await _directMessages[new WebSocketDirectKey(uid, id)].Broadcast(result);
				} catch (Exception e) { Console.WriteLine(e.Message); Console.WriteLine(e.StackTrace); break; }
					 
				await _directMessages[new WebSocketDirectKey(uid, id)].Disconnect(uid);
			}
		else HttpContext.Response.StatusCode = 400;
	}

	private async Task<string> NextMessage(WebSocket ws, byte[] buffer) 
	{
		// empty and prepare the buffer
		Array.Clear(buffer, 0, buffer.Length);

		// receive the message
		await ws.ReceiveAsync(buffer, CancellationToken.None);
		if (ws.State == WebSocketState.CloseReceived) 
			throw new Exception();
		return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
	}
}
