using Microsoft.AspNetCore.Mvc;
using pidgin.models;

namespace pidgin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        this._userService = userModel;
    }

    [HttpGet]
    public Task<User> Get(int id)
    {
        return _userService.GetUserById(id);
    }
}