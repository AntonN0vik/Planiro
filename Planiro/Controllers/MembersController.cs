// Controllers/MembersController.cs

using Microsoft.AspNetCore.Mvc;
using Planiro.Application.Services;

namespace Planiro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController(TeamService teamService) : ControllerBase
{
    // DELETE: api/members/{memberId}
    [HttpDelete("{memberId}")]
    public IActionResult DeleteMember(string memberId, string teamId)
    {
        //TODO 
        var newTeamId = Guid.Parse(teamId);
        var newMemberId = Guid.Parse(memberId);
        var _members = teamService.RemoveMemberAsync(newTeamId, newMemberId);
        return Ok(new { message = "Участник удален" });
    }
    
    [HttpGet("{*url}")]
    public IActionResult Spa()
    {
        return PhysicalFile(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
            "text/html");
    }
}