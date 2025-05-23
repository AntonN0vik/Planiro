namespace Planiro.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    public ICollection<Guid>? Teams { get; set; }

    public ICollection<Guid>? Planners { get; set; }

    public User(Guid id, string firstName, string lastName, string userName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
    }
}