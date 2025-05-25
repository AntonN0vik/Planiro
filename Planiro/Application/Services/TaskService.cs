using Planiro.Domain.IRepositories;
using Planiro.Domain.IRepositories;
using Planiro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetUserTasksAsync(Guid userId)
        {
            return await _taskRepository.GetTaskByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetTeamTasksAsync(Guid teamId)
        {
            return await _taskRepository.GetTaskByTeamIdAsync(teamId);
        }

        public async Task<Domain.Entities.Task> CreateTaskAsync(Domain.Entities.Task task)
        {
            await _taskRepository.SaveTaskAsync(task);
            return task;
        }

        public async Task UpdateTaskAsync(Domain.Entities.Task task)
        {
            await _taskRepository.SaveTaskAsync(task);
        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task != null)
            {
                await _taskRepository.RemoveTaskAsync(task);
            }
        }
    }
}