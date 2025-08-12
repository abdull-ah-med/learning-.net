using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace AuthApp.DTO;

public class SignUpDTO
{
    [Required] public string FullName { get; set; } = string.Empty;
    [Required][EmailAddress] public string Email { get; set; } = string.Empty;
    [Required][StringLength(10, MinimumLength = 3)] public string Username { get; set; } = string.Empty;
    [Required][StringLength(20, MinimumLength = 6)] public string Password { get; set; } = string.Empty;
    public DateTime created { get; set; }
}
