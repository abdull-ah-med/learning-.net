using System;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models;

public class User
{
    public int Id { get; set; }
    [Required][StringLength(30, MinimumLength = 4)] public string FullName { get; set; } = string.Empty;
    [Required] public string PasswordHash { get; set; } = string.Empty;
    [Required][StringLength(30, MinimumLength = 4)] [EmailAddress]public string Email { get; set; } = string.Empty;
    [Required][StringLength(30, MinimumLength = 4)] public string Username { get; set; } = string.Empty;
    [Required] public DateTime created { get; set; }


}
