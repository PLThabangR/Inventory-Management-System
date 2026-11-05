using System;
using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Dtos.Auth;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } 
//
    [Required]
    public string Password { get; set; } 

    public string Role { get; set; } = "User";
}
