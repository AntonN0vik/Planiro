namespace Planiro.Infrastructure.Data.Entities;

public class TaskEntity
{
    public string? Title { get; set; }

    public string? Description { get; set; }
    
    public string? State { get; set; }
    
    public DateTime? Deadline { get; set; }
    
    public bool IsApproved { get; set; } = false;
    
    public Guid UserId;
    
    public UserEntity? User { get; set; }
}