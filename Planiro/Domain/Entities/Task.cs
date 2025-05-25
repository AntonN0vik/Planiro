namespace Planiro.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; }

    public string? Description { get; set; } 
    public enum States
    {
        ToDo, 
        InProgress,
        OnChecking, 
        Done
    }
    
    public States State { get; set; }
    
    public DateTime? Deadline { get; set; }
    
    public bool IsApproved { get; set; } = false;
    
    public Guid PlannerId { get; set; }
}