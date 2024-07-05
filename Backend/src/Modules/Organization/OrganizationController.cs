using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using pidgin.models;
using pidgin.services;

namespace pidgin.Controllers;

[ApiController]
[Route("api")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserService _userService;

    public OrganizationController(IOrganizationService _organizationService, IUserService _userService)
    {
        this._organizationService = _organizationService;
		this._userService = _userService;
    }
	
	[Authorize(AuthenticationSchemes = "Cookies")]
	[HttpPost("GetAllUsersInOrganization"), Produces(MediaTypeNames.Application.Json)]
	public async Task<List<User>> GetAllUsersInOrganization()
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		User u = await _userService.GetUserById(uid);
		List<User> users = await _organizationService.GetAllUsersInOrganization(u.organizationId);
		return users;
	}
}