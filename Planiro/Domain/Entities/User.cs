namespace Planiro.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string UserName { get; private set; }

    public ICollection<Guid>? Teams { get; private set; }

    public ICollection<Guid>? Planners { get; private set; }

    public User(Guid id, string firstName, string lastName, string userName,
        ICollection<Guid>? teams, ICollection<Guid>? planners)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Teams = teams;
        Planners = planners;
    }
}