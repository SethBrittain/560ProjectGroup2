using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Pidgin.Model;

namespace Pidgin.Controller;

[Route("unauthorized")]
public class UnauthorizedController : ControllerBase
{
	[HttpGet("")]
    public async Task<IActionResult> Get()
    {
        return new StatusCodeResult(403);
    }
}