using System.Net.Mime;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pidgin.services;

namespace Pidgin.Modules.Channels;

[ApiController]
[Route("api")]
public class ChannelController : ControllerBase
{
    private readonly IChannelService _channelService;

    public ChannelController(IChannelService channelService)
    {
        _channelService = channelService;
    }

    [HttpGet("Channels/{id}"), Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get(int id)
    {
        Channel c = await _channelService.GetChannelById(id);
        return Ok(c);
    }

    [HttpPost("GetAllChannelsOfUser"), Produces(MediaTypeNames.Application.Json)]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<IActionResult> GetAllChannelsOfUser()
    {
        // Obtain and parse user id from claim string
        string? uidClaim = HttpContext.User.FindFirstValue("uid");
        if (!int.TryParse(uidClaim, out int uid))
            return Unauthorized("User id claim is not a valid integer");

        IEnumerable<Channel> channels = await _channelService.GetAllChannelsOfUser(uid);
        return Ok(channels);
    }

    [HttpPost("GetChannelName")]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public async Task<string> GetChannelName([FromForm] int channelId)
    {
        Channel c = await _channelService.GetChannelById(channelId);
        return c.name;
    }
}