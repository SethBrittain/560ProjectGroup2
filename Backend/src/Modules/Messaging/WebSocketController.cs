using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pidgin.services;
using System.Security.Claims;
using WebSocketManager = Pidgin.Services.WebSocketManager;

namespace Pidgin.Modules.Messaging;

[Controller]
[Route("chat")]
public class ChatController : ControllerBase
{
    /// <summary>
    /// Singleton object for management of websockets
    /// </summary>
    private readonly WebSocketManager _manager;

    /// <summary>
    /// Service for manipulating channel data
    /// </summary>
    private readonly IChannelService _channelService;

    /// <summary>
    /// Service for manipulating message data
    /// </summary>
    private readonly IMessageService _messageService;

    /// <summary>
    /// Constructor for WebSocketController
    /// </summary>
    /// <param name="manager">The websocket managing service</param>
    /// <param name="channelService">The channel managing service</param>
    /// <param name="messageService">The message managing service</param>
    public ChatController(WebSocketManager manager, IChannelService channelService, IMessageService messageService)
    {
        _manager = manager;
        _channelService = channelService;
        _messageService = messageService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> HandleChatConnection([FromQuery] string type, [FromQuery] int id)
    {
        // Reject if not a websocket request
        if (!HttpContext.WebSockets.IsWebSocketRequest)
            return BadRequest("Request must be a websocket connection");

        // Validate chat connection parameters (valid chat type and id > 0)
        try { await ValidateChatConnectionParameters(type, id); } 
        catch (Exception e) { return BadRequest(e.Message); }

        // Obtain and parse user id from claim string
        string? uidClaim = HttpContext.User.FindFirstValue("uid");
        if (!int.TryParse(uidClaim, out int uid))
            return Unauthorized("User id claim is not a valid integer");
        
        // Accept connection and handle it with the WebSocketManager
        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _manager.HandleConnection(uid, type, id, webSocket);

        // Return OK status
        return Ok();
    }

    /// <summary>
    /// Checks that the type of the chat is either 'channel' or 'direct' 
    /// and that the id is greater than 0
    /// </summary>
    /// <param name="type">String designating whether the chat is 'direct' or a 'channel'</param>
    /// <param name="id">Identifier of chat</param>
    /// <exception cref="Exception">Throws if validation fails</exception>
    private async Task ValidateChatConnectionParameters(string type, int id)
    {
        if (type != "channel" && type != "direct")
            throw new Exception($"{type} is not a valid chat type. Please specify either 'channel' or 'direct'");
        if (id < 1)
            throw new Exception("Invalid chat id");
    }
}
