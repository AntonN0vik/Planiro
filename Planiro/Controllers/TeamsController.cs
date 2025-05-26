using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Planiro.Application.Services;
using Planiro.Models;

namespace Planiro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly TeamAuthorizationService _teamAuthService;
    private readonly TeamService _teamService;

    public TeamsController(TeamAuthorizationService teamAuthService, TeamService teamService)
    {
        _teamAuthService = teamAuthService;
        _teamService = teamService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTeam([FromBody] Planiro.Models.CreateTeamRequest request)
    {
        try
        {
            var code = await _teamAuthService.RegisterTeam(request);

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
            var joinRequest = new JoinTeamRequest(
                Code: request.Code.Replace("-", ""),
                Username: request.Username);

            var teamId = await _teamAuthService.AuthorizeTeamAndGetId(joinRequest);

            return Ok(new
            {
                message = "Успешное подключение к команде",
                teamId = teamId.ToString()
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

    // GET: api/teams/{teamId}
    [HttpGet("{teamId}")]
    public async Task<IActionResult> GetTeamData(string teamId)
    {
        //TODO 
        try
        {
            var newTeamId = Guid.Parse(teamId);
            var members = await _teamService.GetTeamMembersAsync(newTeamId);
            var enumerable = members.ToList();
            if (enumerable.Count == 0)
                return Ok(new
                {
                    tasks = new List<TaskShema>(),
                    members = new List<Member>()
                });
            var connectPlannersUsers = enumerable?.Select(x => (x.Id, x.Planners));
            var newMembers = (from member in enumerable
                let id = member.Id.ToString()
                let name = $"{member.FirstName} {member.LastName}"
                select new Member(id, name)).ToList();
            var tasks = await _teamService.GetTasksByTeamIdAsync(newTeamId);

            var newTasks = (from task in tasks
                let id = task.Id.ToString()
                let title = task.Title
                let description = task.Description
                let status = task.State.ToString()
                let assignee = connectPlannersUsers.FirstOrDefault(x => x.Planners.Contains(task.PlannerId)).Id
                    .ToString()
                let deadline = task.Deadline.ToString()
                select new TaskShema(id, title, description, status, assignee, deadline)).ToList();

            return Ok(new
            {
                tasks = newTasks,
                members = newMembers
            });
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex);
            return Conflict();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Conflict();
        }
    }
}