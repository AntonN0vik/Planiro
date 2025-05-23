using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Planiro.Models;

namespace Planiro.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
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
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
            "text/html"
        );
    }
}

// public class HomeController : Controller
// {
//     private readonly ILogger<HomeController> _logger;
//
//     public HomeController(ILogger<HomeController> logger)
//     {
//         _logger = logger;
//     }
//
//     public IActionResult Index()
//     {
//         return View();
//     }
//
//     public IActionResult Privacy()
//     {
//         return View();
//     }
//
//     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//     public IActionResult Error()
//     {
//         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//     }
// }