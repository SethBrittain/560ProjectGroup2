using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using pidgin.models;
using pidgin.services;

namespace pidgin.Controllers;

[Route("unauthorized")]
public class UnauthorizedController : ControllerBase
{
    public UnauthorizedController()
    {
    }

    public async Task<IActionResult> Get()
    {
        Console.WriteLine("Test");
        return new StatusCodeResult(403);
    }
}