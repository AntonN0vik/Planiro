using System.ComponentModel.DataAnnotations;

namespace Planiro.Infrastructure.Data.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    [MaxLength(50)] public string? FirstName { get; set; }
    [MaxLength(50)] public string? LastName { get; set; }


    [MaxLength(50)] public string? UserName { get; set; }
    [MaxLength(512)] public string? PasswordHash { get; set; }
    public ICollection<TeamEntity>? Teams { get; set; }
    public ICollection<TaskEntity>? Tasks { get; set; }
    public ICollection<PlanerEntity>? Planers { get; set; }
   
}