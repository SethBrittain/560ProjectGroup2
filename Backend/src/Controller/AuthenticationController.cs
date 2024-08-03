using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Pidgin.Model;
using Pidgin.Repository;
using Pidgin.Services;

namespace Pidgin.Controller;

[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
	private const string emailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
									+ @"@"
									+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
	
    private string AUTH_SERVICE_HOST = Environment.GetEnvironmentVariable("ASPNETCORE_AUTH_SERVICE_HOST") ?? throw new Exception("AUTH_SERVICE_HOST environment variable not set");
    private string AUTH_CAS_HOST = Environment.GetEnvironmentVariable("ASPNETCORE_AUTH_CAS_HOST") ?? throw new Exception("AUTH_CAS_HOST environment variable not set");

	private readonly IPasswordService _passwordService;

	private readonly IObjectRepository<User> _userRepository;

    public AuthenticationController(IPasswordService passwordService, IObjectRepository<User> userRepository)
    {
		_passwordService = passwordService;
		_userRepository = userRepository;
    }

	[AllowAnonymous]
	[HttpPost("signup")]
	public async Task<IActionResult> SignUp(
		[FromForm] string email, 
		[FromForm] string password, 
		[FromForm] string confirmPassword, 
		[FromForm] string firstName, 
		[FromForm] string lastName
	)
	{
		if (Regex.IsMatch(email, emailRegex) == false)
			return BadRequest("Invalid email address");
		if (password != confirmPassword)
			return BadRequest("Passwords do not match");
		if (firstName.Length == 0 || lastName.Length == 0)
			return BadRequest("First and last name are required");
		if (!_passwordService.IsValidPassword(password))
			return BadRequest("Password does not meet requirements");
		if (await _passwordService.EmailExists(email))
			return BadRequest("User with that e-mail already exists");
		
		string hashedPassword = _passwordService.HashPassword(password);
		
		User u = new User(
			organizationId: 1,
			email: email,
			firstName: firstName,
			lastName: lastName,
			active: true,
			password: hashedPassword
		);

		await _userRepository.Create(u);

		return Ok();
	}

	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
	{
		int uid = await _passwordService.ValidateCredentials(email, password);
		if (uid == 0) return Forbid();
		
		User u = await _userRepository.Get(uid,uid);

		List<Claim> userClaims = new() {
			new Claim("uid", u.id.ToString()),
			new Claim(ClaimTypes.Email, email),
			new Claim(ClaimTypes.Name, $"{u.firstName} {u.lastName}"),
			new Claim(ClaimTypes.Role, "User")
		};
		
		ClaimsIdentity userIdentity = new ClaimsIdentity(userClaims, "User Identity");
		ClaimsPrincipal userPrinciple = new ClaimsPrincipal(new[] {userIdentity});

		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,userPrinciple);

		return Ok();
	}

    // [AllowAnonymous]
	// [HttpGet("cas/login")]
    // public IActionResult CasLogin(string returnUrl)
    // {
    //     string url = $"{AUTH_CAS_HOST}/login?service={Uri.EscapeDataString(AUTH_SERVICE_HOST)}ticket";
    //     return Redirect(url);           
    // }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/"); 
    }

	// [AllowAnonymous]
    // [HttpGet("cas/ticket")]
    // public async Task<IActionResult> Ticket([FromQuery] string ticket)
    // {
    //     string url = $"{AUTH_CAS_HOST}serviceValidate?ticket={ticket}&service={Uri.EscapeDataString(AUTH_SERVICE_HOST)}ticket";
        
    //     HttpClient http = new();
    //     using (HttpResponseMessage response = await http.GetAsync(url))
    //     using (Stream stream = await response.Content.ReadAsStreamAsync())
	// 	using (StreamReader reader = new StreamReader(stream))
	// 	{
	// 		string body = await reader.ReadToEndAsync();
	// 		XDocument doc = XDocument.Parse(body);
	// 		var element = doc.Descendants("{http://www.yale.edu/tp/cas}user").FirstOrDefault();

	// 		if (element == null)
	// 			return StatusCode(403);
			
	// 		string eid = element.Value;
	// 		string email = $"{eid}@ksu.edu";

	// 		User u = await GetOrCreateUser(email);
			
	// 		List<Claim> userClaims = new() {
	// 			new Claim("uid", u.id.ToString()),
	// 			new Claim(ClaimTypes.Email, email),
	// 			new Claim(ClaimTypes.Name, $"{u.firstName} {u.lastName}"),
	// 			new Claim(ClaimTypes.Role, "User")
	// 		};
			
	// 		ClaimsIdentity userIdentity = new ClaimsIdentity(userClaims, "User Identity");
	// 		ClaimsPrincipal userPrinciple = new ClaimsPrincipal(new[] { userIdentity });

	// 		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,userPrinciple);

	// 		return Redirect("/app");
	// 	}

    // }
}