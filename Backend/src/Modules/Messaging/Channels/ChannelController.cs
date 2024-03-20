using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pidgin.services;

namespace Pidgin.Modules.Messaging.Channels;

[ApiController]
[Route("api/")]
public class ChannelController : ControllerBase
{
    private readonly IChannelService _channelService;

    public ChannelController(IChannelService channelService)
    {
        _channelService = channelService;
    }

    [HttpGet("{id}"), Produces(MediaTypeNames.Application.Json)]
    public async Task<Channel> Get(int id)
    {
        Channel c = await _channelService.GetChannelById(id);
        return c;
    }

    [HttpPost("GetAllChannelsOfUser"), Produces(MediaTypeNames.Application.Json)]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<List<Channel>> GetAllChannelsOfUser()
    {
        ClaimsPrincipal u = HttpContext.User;
        Console.WriteLine("claims:");
        Console.WriteLine(u.FindFirstValue("uid"));
        List<Channel> channels = await _channelService.GetAllChannelsOfUser(int.Parse(u.FindFirstValue("uid")));
        return channels;
    }
}