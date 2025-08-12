// using System;
// using AuthApp.Models;
// using AuthApp.Data;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using System.Security.Claims;
// using Microsoft.AspNetCore.Mvc;

// namespace AuthApp.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class AuthController(AppDbContext db) : ControllerBase
// {
//     [HttpPost("signup")]
//     public async Task<IActionResult> Signup([FromBody] User user)
//     {
//         if (await db.Users.AnyAsync(u => u.Username == user.Username))
//         {
//             return BadRequest("User Already Exists");
//         }
//         var newUser = new User
//         {
//             Username = user.Username,
//             PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash)
//         };
//         db.Users.Add(newUser);
//         await db.SaveChangesAsync();
//         return Ok("User Created");
//     }
//     [HttpPost("signin")]
//     public async Task<IActionResult> Login([FromBody] User user)
//     {
//         var dbUser = await db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
//         if (dbUser == null) return Unauthorized("Invalid Credentials");

//         var isValid = BCrypt.Net.BCrypt.Verify(user.PasswordHash, dbUser.PasswordHash);
//         if (!isValid)
//         {
//             return Unauthorized("Invalid Credentials");
//         }

//         var claims = new List<Claim>
//         {
//             new(ClaimTypes.Name, dbUser.Username),
//             new(ClaimTypes.NameIdentifier, dbUser.Id.ToString())
//         };
//         var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//         var principal = new ClaimsPrincipal(identity);
//         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
//         return Ok("Logged in");
//     }
//     [HttpGet("me")]
//     public IActionResult Me()
//     {
//         if (!User.Identity?.IsAuthenticated ?? true)

//             return Unauthorized();

//         return Ok(new { Username = User.Identity?.Name });
//     }
// }
