namespace Planiro.Domain.Entities;

public class Task
{
	public enum States
	{
		ToDo,
		InProgress,
		OnChecking,
		Done
	}
	
    public Guid Id { get; private set; }

    public string? Title { get; private set; }
    
    public string? Description { get; private set; }
    
    public States State { get; private set; }

    public DateTime? Deadline { get; private set; }

    public bool IsApproved { get; private set; } = false;

    public Guid PlannerId { get; private set; }

    public Task(Guid id, string? title, string? description, States state,
                DateTime? deadline, Guid plannerId,
                bool isApproved = false)
    {
        Id = id;
        Title = title;
        Description = description;
        State = state;
        Deadline = deadline;
        PlannerId = plannerId;
        IsApproved = isApproved;
    }
}