using Microsoft.AspNetCore.Mvc;
using Planiro.Application.Services;
using Planiro.Models;

namespace Planiro.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private static List<TaskShema> _tasks = new List<TaskShema>
    {
        new TaskShema(
            "1", 
            "Пример задачи", 
            "Описание задачи",
            "To Do",
            "1",
            DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")
        )
    };

    private static List<Member> _members = new List<Member>
    {
        new Member("1", "Тестовый участник" )
    };
    private readonly TeamAuthorizationService _teamService;

    public TeamsController(TeamAuthorizationService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTeam([FromBody] Planiro.Models.CreateTeamRequest request)
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
            var testTeamId = "2afc3bf5-4c11-4d58-b65e-a8c3bdbaeda1";
        
            return Ok(new { message = "Успешное подключение к команде", teamId = testTeamId});
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
    
    // GET: api/teams/{teamId}
    [HttpGet("{teamId}")]
    public IActionResult GetTeamData(string teamId)
    {
        //TODO 
        return Ok(new
        {
            tasks = _tasks,
            members = _members
        });
    }
    
}