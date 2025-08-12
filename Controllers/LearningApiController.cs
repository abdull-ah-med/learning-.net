using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApp.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Authentication;
using AuthApp.DTO;
using Microsoft.Extensions.Options;
using AuthApp.Options;

namespace AuthApp.Controllers;

[ApiController]
[Route("api/auth")]
public class LearningApiController(IUserService US, ISystemCLK time) : ControllerBase
{

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInDTO User)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            User? ValidatedUser = null;
            ValidatedUser = User.LoginIdentifier.Contains('@') ?
             await US.ValidateUserEmailAsync(User.LoginIdentifier, User.Password)
             : await US.ValidateUserUsernameAsync(User.LoginIdentifier, User.Password);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, ValidatedUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, ValidatedUser.Username),
                    new Claim(ClaimTypes.Email, ValidatedUser.Email)
                };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Ok("User is Logged In");


        }
        catch (InvalidCredentialException)
        {
            return Unauthorized("Invalid Credentials");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");

        }
        {// var userExists = await Db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
         // if (userExists == null)
         // {
         //     return BadRequest("Invalid Credentials");
         // }
         // bool PassMatch = BCrypt.Net.BCrypt.Verify(user.PasswordHash, userExists.PasswordHash);
         // if (!PassMatch)
         // {
         //     return BadRequest("Invalid Credentials");
         // }
         // var claims = new List<Claim>
         // {
         //     new Claim(ClaimTypes.NameIdentifier, userExists.Id.ToString()),
         //     new Claim(ClaimTypes.Name, userExists.Username)
         // };
         // var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
         // var principal = new ClaimsPrincipal(identity);
         // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
         // return Ok("User Logged In");
        }
    }
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO _signUpDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var userCreated = await US.CreateUserAsync(_signUpDto.FullName, _signUpDto.Username, _signUpDto.Email, _signUpDto.Password, time.TimeNow());
            if (userCreated != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userCreated.Id.ToString()),
                    new Claim(ClaimTypes.Name, userCreated.Username),
                    new Claim(ClaimTypes.Email, userCreated.Email)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Ok("User is created");
            }
            else
            {
                return BadRequest("Internal Server Error");
            }

        }
        catch (UserAlreadyExistsException)
        {
            return Conflict("Duplicate Credential Exists");
        }
        catch (InternalServerErrorException)
        {
            return StatusCode(500, "Internal Server Error");
        }
        {
            // var username = _signUpDto.Username;
            // var dbUser = await Db.Users.FirstOrDefaultAsync(u => u.Username == username);
            // if (dbUser != null)
            // {
            //     return Conflict("Username already exists");
            // }
            // var HashedPassword = BCrypt.Net.BCrypt.HashPassword(_signUpDto.PasswordHash);
            // var newUser = new _signUpDto
            // {
            //     Username = _signUpDto.Username,
            //     PasswordHash = HashedPassword
            // };
            // await Db.Users.AddAsync(newUser);
            // await Db.SaveChangesAsync();
        }

    }
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetUserData()
    {
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        return Ok(new { userName, userEmail });
    }
}
