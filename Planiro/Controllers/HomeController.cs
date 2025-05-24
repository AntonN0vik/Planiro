using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Planiro.Models;

namespace Planiro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        // TODO валидаци и логика регистрации из APLICATION
        return Ok(new { Message = "regiser sucess" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // TODO валидаци и логика входа из APLICATION
        return Ok(new { Message = "login success" });
    }

    // SPA fallback (должен быть последним)
    [HttpGet("{*url}")]
    public IActionResult Spa()
    {
        return PhysicalFile(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                "index.html"),
            "text/html"
        );
    }
}

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private static Dictionary<string, string> teams = new();

    [HttpPost("create")]
    public IActionResult CreateTeam([FromBody] CreateTeamRequest request)
    {
        var code = Guid.NewGuid().ToString("N")[..8].ToUpper(); // заглушка
        // TODO СОЗДАНИЕ КОМАНДЫ И Гнерация кода

        return Ok(new
        {
            code = $"{code.Substring(0, 4)}-{code.Substring(4, 4)}",
            message = "Команда успешно создана"
        });
    }

    [HttpPost("join")]
    public IActionResult JoinTeam([FromBody] JoinRequest request)
    {
        // TODO Добавление в команду
        return Ok(new
        {
            message = "Успешное подключение к команде",
        });
    }

    // SPA fallback (должен быть последним)
    [HttpGet("{*url}")]
    public IActionResult Spa()
    {
        return PhysicalFile(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                "index.html"),
            "text/html"
        );
    }
}

public class CreateTeamRequest
{
    public string Username { get; set; }
}

public class JoinRequest
{
    public string Code { get; set; }
    public string Username { get; set; }
}