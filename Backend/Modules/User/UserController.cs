using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using pidgin.models;
using pidgin.services;

namespace pidgin.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        this._userService = userService;
    }
	
    [HttpGet("{id}"), Produces(MediaTypeNames.Application.Json)]
    public async Task<User> Get(int id)
    {
        User u = await _userService.GetUserById(id);
        return u;
    }
}