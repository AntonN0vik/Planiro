namespace Planiro.Domain.IRepositories;

using Task = System.Threading.Tasks.Task;
using TaskDomain = Planiro.Domain.Entities.Task;

public interface ITaskRepository
{
    Task<ICollection<TaskDomain>> GetTaskByUserIdAsync(Guid userId);
    
    Task<ICollection<TaskDomain>> GetTaskByTeamIdAsync(Guid teamId);
    
    Task SaveTaskAsync(TaskDomain task);
    
    Task RemoveTaskAsync(TaskDomain task);
    Task<TaskDomain> GetTaskByIdAsync(Guid taskId);
}