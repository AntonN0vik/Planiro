namespace Planiro.Infrastructure.Data.Entities;

public class PlannerEntity
{
    public Guid Id { get; set; }
    public UserEntity? User { get; set; }
    public Guid UserId { get; set; }
    public TeamEntity? Team { get; set; }
    public Guid TeamId { get; set; }
    public ICollection<TaskEntity>? ToDoTasks { get; set; }
    
    public ICollection<TaskEntity>? InProgressTasks { get; set; }
    
    public ICollection<TaskEntity>? OnCheckingTasks { get; set; }
    
    public ICollection<TaskEntity>? DoneTasks { get; set; }
}