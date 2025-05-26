namespace Planiro.Infrastructure.Data.Entities;

public class TeamEntity
{
    public Guid Id { get; set; }
    
    public string? JoinCode { get; set; }
    
    public Guid TeamleadId { get; set; }
    public UserEntity? Teamlead { get; set; }
    
    public ICollection<UserEntity>? Users { get; set; } 
    public ICollection<PlannerEntity>? Planners { get; set; }

}