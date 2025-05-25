// Controllers/TasksController.cs
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Planiro.Models;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    // Тестовые данные в памяти
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
    
    // PUT: api/tasks/{taskId}
    [HttpPut("{taskId}")]
    public IActionResult UpdateTask(string taskId, [FromBody] TaskShema updatedTask)
    {
        //TODO 
        var index = _tasks.FindIndex(t => t.Id == taskId);
        if (index == -1) return NotFound();

        _tasks[index] = updatedTask;
        return Ok(updatedTask);
    }
    

    // POST: api/tasks
    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskRequest request)
    {
        //TODO 
        var newTask = new TaskShema(
            Guid.NewGuid().ToString(),
            request.Title,
            request.Description,
            "To Do",
            request.Assignee,
            request.Deadline
        );

        _tasks.Add(newTask);
        return Ok(newTask);
    }
    [HttpGet("{*url}")]
    public IActionResult Spa()
    {
        return PhysicalFile(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
            "text/html");
    }
}