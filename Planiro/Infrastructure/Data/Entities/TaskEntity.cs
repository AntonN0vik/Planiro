namespace Planiro.Infrastructure.Data.Entities;

public class TaskEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }

    public string? Description { get; set; }
    
    public string? State { get; set; }
    
    public DateTime? Deadline { get; set; }
    
    public bool IsApproved { get; set; } = false;
    
    public Guid PlanerId;
    public PlanerEntity Planer { get; set; }
}