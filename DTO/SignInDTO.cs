using System;
using System.ComponentModel.DataAnnotations;
namespace AuthApp.DTO;

public class SignInDTO
{
    [Required] public string LoginIdentifier { get; set; } = string.Empty;
    [Required][StringLength(20, MinimumLength =6)] public string Password { get; set; } = string.Empty;
}
