using Microsoft.AspNetCore.Mvc;
using Planiro.Application.Services;
using Planiro.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController(UserAuthorizationService _userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            await _userService.RegisterUser(request);
            return Ok(new { Message = "User registered successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            await _userService.AuthorizeUser(request);
            var userId=await _userService.GetUserIdByUserName(request.Username);
            var userName=request.Username;
            
            return Ok(new { Message = "Login successful", userId=userId.ToString()});
        }
        catch (ArgumentException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    [HttpGet("{*url}")]
    public IActionResult Spa()
    {
        return PhysicalFile(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
            "text/html");
    }
}