using System;
using LogiTrack.Models.Auth;

namespace LogiTrack.Service;

public interface IJwtTokenService
{

    Task<string> GenerateToken(ApplicationUser user);

}
