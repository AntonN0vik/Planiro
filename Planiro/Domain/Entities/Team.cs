namespace Planiro.Domain.Entities;

public class Team
{
    public Guid Id { get; private set; }
    
    public string? JoinCode { get; private set; }
    
    public ICollection<Guid>? Collaborators { get; private set; }
    
    public Guid TeamleadId { get; private set; }
    
    public ICollection<Guid>? Planners { get; private set; }
    
    public Team(Guid id, string? joinCode, ICollection<Guid>? collaborators, Guid teamleadId, ICollection<Guid>? planners)
    {
        this.Id = id;
        this.JoinCode = joinCode;
        this.Collaborators = collaborators;
        this.TeamleadId = teamleadId;
        this.Planners = Planners;
    }
}