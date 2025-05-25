namespace Planiro.Domain.IRepositories;

using Task = System.Threading.Tasks.Task;
using TaskDomain = Planiro.Domain.Entities.Task;

public interface ITaskRepository
{   
    Task<TaskDomain> GetTaskByIdAsync(Guid taskId);
    
    Task<ICollection<TaskDomain>> GetTaskByUserIdAsync(Guid plannerId);
    
    Task SaveTaskAsync(TaskDomain task);
    
    Task RemoveTaskAsync(TaskDomain task);
}