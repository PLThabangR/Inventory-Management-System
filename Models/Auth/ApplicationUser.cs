using System;
using Microsoft.AspNetCore.Identity;

namespace LogiTrack.Models.Auth;

//Inherit from Identity user
public class ApplicationUser:IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

}
