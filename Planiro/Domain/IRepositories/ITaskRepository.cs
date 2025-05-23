namespace Planiro.Domain.IRepositories;
using Planiro.Domain;
public interface ITaskRepository
{
    public ICollection<Task>? GetTaskByUserId(Guid userId);
    
    public ICollection<Task>? GetTaskByTeamId(Guid teamId);
    
    public void SaveTask(Task task);
    
    public void RemoveTask(Task task);
}