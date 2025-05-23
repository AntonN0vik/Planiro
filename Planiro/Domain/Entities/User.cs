namespace Planiro.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public enum Roles
    {
        Teamlead,
        Collaborator
    }

    public Roles Role { get; set; }
    
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public ICollection<Guid>? Teams { get; set; }

    public ICollection<Guid>? Planners { get; set; }
}