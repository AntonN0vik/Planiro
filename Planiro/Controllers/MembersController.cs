// Controllers/MembersController.cs

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Planiro.Models;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private static List<Member> _members = new List<Member>
    {
        new Member("1", "Тестовый участник" )
    };

    // DELETE: api/members/{memberId}
    [HttpDelete("{memberId}")]
    public IActionResult DeleteMember(string memberId)
    {
        //TODO 
        var member = _members.FirstOrDefault(m => m.Id == memberId);
        if (member == null) return NotFound();

        _members.Remove(member);
        
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