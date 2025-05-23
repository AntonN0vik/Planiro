namespace Planiro.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    
    public string? JoinCode { get; set; }
    
    public ICollection<Guid>? Collaborators { get; set; }
    
    public Guid TeamleadId { get; set; }
}