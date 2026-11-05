using System;

namespace LogiTrack.Dtos.Auth;

public class AuthResponseDto
{
    public string Token { get; set; }

    public string Email { get; set; } 

    public IList<string> Roles { get; set; } = [];

}
