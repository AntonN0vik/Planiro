namespace Planiro.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    
    public string? JoinCode { get; set; }
    
    public ICollection<Guid>? Collaborators { get; set; }
    
    public Guid TeamleadId { get; set; }
    
    public Team(Guid id, string? joinCode, ICollection<Guid>? collaborators, Guid teamleadId)
    {
        this.Id = id;
        this.JoinCode = joinCode;
        this.Collaborators = collaborators;
        this.TeamleadId = teamleadId;
    }

}