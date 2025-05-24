using Microsoft.AspNetCore.Mvc;
using Planiro.Application.Services;
using Planiro.Models;

namespace Planiro.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly TeamAuthorizationService _teamService;

    public TeamsController(TeamAuthorizationService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTeam(Planiro.Models.CreateTeamRequest request)
    {
        try
        {
            var code = await _teamService.RegisterTeam(request);
            
            return Ok(new
            {
                code = $"{code.Substring(0, 4)}-{code.Substring(4, 4)}",
                message = "Команда успешно создана"
            });
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

    [HttpPost("join")]
    public async Task<IActionResult> JoinTeam([FromBody] JoinRequest request)
    {
        try
        {
            // Создаем JoinTeamRequest через конструктор
            var joinRequest = new JoinTeamRequest(
                Code: request.Code.Replace("-", ""),
                Username: request.Username);

            await _teamService.AuthorizeTeam(joinRequest);
        
            return Ok(new { message = "Успешное подключение к команде" });
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

    [HttpGet("{*url}")]
    public IActionResult Spa()
    {
        return PhysicalFile(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
            "text/html");
    }
}