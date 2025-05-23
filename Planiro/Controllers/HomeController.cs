using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Planiro.Models;

namespace Planiro.Controllers;

[ApiController]
[Route("[controller]")]
public class AppController : ControllerBase
{
    // API endpoint
    [HttpGet("api/data")]
    public IActionResult GetData()
    {
        return Ok(new { Message = "Данные API" });
    }

    // SPA fallback
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