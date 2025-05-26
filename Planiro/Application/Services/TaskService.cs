using Planiro.Domain.IRepositories;
using Planiro.Domain.IRepositories;
using Planiro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Application.Services;

public class TaskService(ITaskRepository taskRepository)
{
    public async Task<IEnumerable<Domain.Entities.Task>> GetUserTasksAsync(Guid userId)
    {
        return await taskRepository.GetTaskByUserIdAsync(userId);
    }

    // public async Task<IEnumerable<Domain.Entities.Task>> GetTeamTasksAsync(Guid teamId)
    // {
    //     return await _taskRepository.GetTaskByTeamIdAsync(teamId);
    // }

    public async Task<Domain.Entities.Task> CreateTaskAsync(Domain.Entities.Task task)
    {
        await taskRepository.SaveTaskAsync(task);
        return task;
    }

    public async Task UpdateTaskAsync(Domain.Entities.Task task)
    {
        await taskRepository.SaveTaskAsync(task);
    }

    public async Task DeleteTaskAsync(Guid taskId)
    {
        var task = await taskRepository.GetTaskByIdAsync(taskId);
        await taskRepository.RemoveTaskAsync(task);
    }
}