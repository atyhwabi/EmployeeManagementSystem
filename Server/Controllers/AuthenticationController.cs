using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ServerLibrary.Services.Interfaces;
using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using BaseLibrary.Entities;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(Registration user)
        {
            if(user is null) return BadRequest(new GeneralResponse(false, "User cannot be null"));

            var response = await userAccount.CreateAsync(user);
            if (response.Flag) return Ok(response);
            return BadRequest(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login user)
        {
            if(user is null) return BadRequest(new GeneralResponse(false, "User cannot be null"));

            var response = await userAccount.LoginAsync(user);
            if (response.Flag) return Ok(response);
            return BadRequest(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshToken token)
        {
            if(token is null) return BadRequest(new GeneralResponse(false, "Token cannot be null"));

            var response = await userAccount.RefreshTokenAsync(token);
            if (response.Flag) return Ok(response);
            return BadRequest(response);
        }
    }
}
