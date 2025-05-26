using System.ComponentModel.DataAnnotations.Schema;

namespace Planiro.Infrastructure.Data.Entities;

public class TaskEntity
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? State { get; set; }
    public DateTime? Deadline { get; set; }
    public bool IsApproved { get; set; }
    
    public Guid PlannerId{ get; set; }
    [ForeignKey("PlannerId")]
    public PlannerEntity Planner { get; set; }
}