using System.Net.Mime;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pidgin.services;

namespace Pidgin.Modules.Messaging.Channels;

[ApiController]
[Route("api")]
public class ChannelController : ControllerBase
{
    private readonly IChannelService _channelService;

    private readonly Pidgin.Services.WebSocketManager _manager;

    public ChannelController(IChannelService channelService, Pidgin.Services.WebSocketManager manager)
    {
        _channelService = channelService;
        _manager = manager;
    }

    [HttpGet("Channels/{id}"), Produces(MediaTypeNames.Application.Json)]
    public async Task<Channel> Get(int id)
    {
        Channel c = await _channelService.GetChannelById(id);
        return c;
    }

    [HttpGet("Channels/{id}/ws")]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task WebsocketConnect(int id)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));

            while (webSocket.State == WebSocketState.Open)
            {
                _manager.HandleMessages(id, uid, webSocket);
            }
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    [HttpPost("GetAllChannelsOfUser"), Produces(MediaTypeNames.Application.Json)]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IEnumerable<Channel>> GetAllChannelsOfUser()
    {
        ClaimsPrincipal u = HttpContext.User;
        Console.WriteLine("claims:");
        Console.WriteLine(u.FindFirstValue("uid"));
        IEnumerable<Channel> channels = await _channelService.GetAllChannelsOfUser(int.Parse(u.FindFirstValue("uid")));
        return channels;
    }

    [HttpPost("GetChannelName")]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<string> GetChannelName([FromForm] int channelId)
    {
        Channel c = await _channelService.GetChannelById(channelId);
        return c.name;
    }
}