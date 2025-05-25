namespace Planiro.Domain.Entities;

public class Planner
{
    public Guid Id { get; private set; }
    
    public Guid TeamId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public ICollection<Guid>? ToDoTasks { get; private set; }
    
    public ICollection<Guid>? InProgressTasks { get; private set; }
    
    public ICollection<Guid>? OnCheckingTasks { get; private set; }
    
    public ICollection<Guid>? DoneTasks { get; private set; }

    public Planner(Guid id, Guid teamId, Guid userId, 
        ICollection<Guid>? toDoTasks,  ICollection<Guid>? inProgressTasks,
        ICollection<Guid>? onCheckingTasks, ICollection<Guid>? doneTasks)
    {
        Id = id;
        TeamId = teamId;
        UserId = userId;
        ToDoTasks = toDoTasks;
        InProgressTasks = inProgressTasks;
        OnCheckingTasks = onCheckingTasks;
        DoneTasks = doneTasks;
    }
}