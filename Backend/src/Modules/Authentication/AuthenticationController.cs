
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pidgin.Exceptions;
using pidgin.models;
using pidgin.services;

namespace pidgin.Controllers;

[Route("auth")]
public class AuthenticationController : ControllerBase
{
    const string AUTH_SERVICE_HOST = "http://localhost/auth/cas/";
    const string AUTH_CAS_HOST = "https://signin.k-state.edu/WebISO/";

    private IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
	[Route("cas/login")]
    public IActionResult Login(string returnUrl)
    {
        string url = $"{AUTH_CAS_HOST}/login?service={Uri.EscapeDataString(AUTH_SERVICE_HOST)}ticket";
        return Redirect(url);           
    }

    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/"); 
    }

	[AllowAnonymous]
    [Route("cas/ticket")]
    public async Task<IActionResult> Ticket(string ticket)
    {
        string url = $"{AUTH_CAS_HOST}serviceValidate?ticket={ticket}&service={Uri.EscapeDataString(AUTH_SERVICE_HOST)}ticket";
        
        HttpClient http = new();
        using (HttpResponseMessage response = await http.GetAsync(url))
        {
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string body = await reader.ReadToEndAsync();
                    XDocument doc = XDocument.Parse(body);
                    var element = doc.Descendants("{http://www.yale.edu/tp/cas}user").FirstOrDefault();

                    if(element != null)
                    {
                        string eid = element.Value;
						string email = $"{eid}@ksu.edu";

						User u;
						try 
						{
							u = await this._userService.GetUserByEmail(email);
						} catch (UserNotFoundException) {
							u = await this._userService.RegisterUser(1, email, "test", "user", "student", "");
						}
						
                        List<Claim> userClaims = new() {
							new Claim("uid", u.id.ToString()),
							new Claim(ClaimTypes.Email, email),
							new Claim(ClaimTypes.Name, $"{u.firstName} {u.lastName}"),
							new Claim(ClaimTypes.Role, "User")
                        };
						
                        ClaimsIdentity userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                        ClaimsPrincipal userPrinciple = new ClaimsPrincipal(new[] { userIdentity });

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,userPrinciple);

                        return Redirect("/app");
                    }
                }
            }
        }

        return StatusCode(403);
    }
}