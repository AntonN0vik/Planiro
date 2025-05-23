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
    
    public Guid TeamId { get; set; } // чтобы установить полное соотвествие между задачей и доской
    
    public TeamEntity? Team { get; set; }
}