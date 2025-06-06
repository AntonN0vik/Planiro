﻿using System.Globalization;
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
    private readonly PlannerService _plannerService;

    public TeamsController(TeamAuthorizationService teamAuthService, TeamService teamService, PlannerService plannerService)
    {
        _teamAuthService = teamAuthService;
        _teamService = teamService;
        _plannerService = plannerService;
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
                Code: request.Code.Remove(4,1),
                Username: request.Username);

            var teamId = await _teamAuthService.AuthorizeTeamAndGetId(joinRequest);
            await _plannerService.CreatePlanner(request.Code.Remove(4,1), request.Username);
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
        try
        {
            var newTeamId = Guid.Parse(teamId);
            var members = await _teamService.GetTeamMembersAsync(newTeamId);
            var teamleadId = await _teamService.GetTeamLeadIdAsync(newTeamId);
            var enumerable = members.ToList();
            var joinCode = await _teamService.GetJoinCodeByTeamIdAsync(newTeamId);
            if (enumerable.Count == 0)
                return Ok(new
                {
                    tasks = new List<TaskShema>(),
                    members = new List<Member>(),
                    lead = teamleadId,
                    joinCode = joinCode
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
                members = newMembers,
                lead = teamleadId,
                joinCode = joinCode
            });
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            return Conflict();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Conflict();
        }
    }
}