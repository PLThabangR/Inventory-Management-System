using System.Security.Claims;
using LogiTrack.Dtos.Auth;
using LogiTrack.Models.Auth;
using LogiTrack.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtTokenService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

            if(existingUser != null){
                return Conflict(new {
                    message= "An Account with this email already exists"
                });
            }
            //Create user object 
                 var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email
        };
        //Create user using userManager

            var result = await _userManager.CreateAsync(
                      user,
                      registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => e.Description)
                });
            }
            // 2. Validate the requested role
            var role = registerDto.Role;

            if (role != "User" && role != "Manager")
            {
                // Remove the newly created user because the role is invalid
                await _userManager.DeleteAsync(user);

                return BadRequest(new
                {
                    message = "Invalid role.Either User or Manager"
                });
            }

            // Every newly registered user gets the User role.
            var roleResult=  await _userManager.AddToRoleAsync(user, role);

            //Lets check if role creation is success
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return StatusCode(500, new
                {
                    errors = roleResult.Errors.Select(e => e.Description)
                });
            }

            return Ok(new
            {
                message = "Registration successful.",
                email = user.Email,
                role
            });

        }


        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(
            LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,lockoutOnFailure:true);

            if (!result.Succeeded)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

           //We generate the token 
           var token = await _jwtService.GenerateToken(user);

           //Retireve roles for response 
           var roles = await _userManager.GetRolesAsync(user);
           return Ok(new AuthResponseDto
           {
              Token=token,
              Email =user.Email!,
              Roles=roles

           });


        }

        }
    }
