// Controllers/TasksController.cs

using Microsoft.AspNetCore.Mvc;
using Planiro.Application.Services;
using Planiro.Models;
using Planiro.Domain.Entities;
using Task = Planiro.Domain.Entities.Task;

namespace Planiro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(TaskService taskService, TeamService teamService) : ControllerBase
{
    private readonly TeamService _teamService = teamService;


    // PUT: api/tasks/{taskId}
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(string taskId, [FromBody] TaskShema updatedTask, string teamId)
    {
        try
        {
            var newTeamId = Guid.Parse(teamId);
            var newUserId = Guid.Parse(updatedTask.Assignee);
            var newTaskId = Guid.Parse(taskId);
            var newTitle = updatedTask.Title;
            var newDescription = updatedTask.Description;
            var newDeadline = DateTime.Parse(updatedTask.Deadline);
            var plannerId = await _teamService.GetPlannerIdAsync(newTeamId, newUserId);
            var newStatus = Enum.Parse<Task.States>(updatedTask.Status);
            var task = new Task(newTaskId, newTitle, newDescription, newStatus, newDeadline, plannerId);
            await taskService.UpdateTaskAsync(task);
            return Ok(updatedTask);
        }
        catch (ArgumentException ex)
        {
            return Conflict(ex);
        }
    }


    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskRequest request, string teamId)
    {
        try
        {
            var taskId = Guid.NewGuid();
            var title = request.Title;
            var description = request.Description;
            var deadline = DateTime.Parse(request.Deadline);
            var newTeamId = Guid.Parse(teamId);
            var newUserId = Guid.Parse(request.Assignee);
            var plannerId = await _teamService.GetPlannerIdAsync(newTeamId, newUserId);
            
            var newTask = new Task(taskId, title, description, 
                Task.States.ToDo, deadline, plannerId);
            
            return Ok(newTask);
        }
        catch (ArgumentException ex)
        {
            return Conflict(ex);
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