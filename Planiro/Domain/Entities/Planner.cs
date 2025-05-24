namespace Planiro.Domain.Entities;

public class Planner
{
    public Guid Id { get; set; }
    
    public Guid TeamId { get; set; }
    
    public Guid UserId { get; set; }
    
    public ICollection<Guid>? ToDoTasks { get; set; }
    
    public ICollection<Guid>? InProgressTasks { get; set; }
    
    public ICollection<Guid>? OnCheckingTasks { get; set; }
    
    public ICollection<Guid>? DoneTasks { get; set; }
}