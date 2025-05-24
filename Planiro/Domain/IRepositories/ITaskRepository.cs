namespace Planiro.Domain.IRepositories;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Planiro.Domain.Entities.Task;
using Planiro.Domain;
public interface ITaskRepository
{
    Task<ICollection<TaskEntity>> GetTaskByUserIdAsync(Guid userId);
    
    Task<ICollection<TaskEntity>> GetTaskByTeamIdAsync(Guid teamId);
    
    Task SaveTaskAsync(TaskEntity task);
    
    Task RemoveTaskAsync(TaskEntity task);
}