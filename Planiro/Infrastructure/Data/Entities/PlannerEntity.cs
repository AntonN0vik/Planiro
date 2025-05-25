using System.ComponentModel.DataAnnotations.Schema;

namespace Planiro.Infrastructure.Data.Entities;

public class PlannerEntity
{
    public Guid Id { get; set; }
    public UserEntity? User { get; set; }
    public Guid UserId { get; set; }
    public TeamEntity? Team { get; set; }
    public Guid TeamId { get; set; }
    
    public ICollection<TaskEntity>? Tasks { get; set; }
    
    [NotMapped]
    public IEnumerable<TaskEntity>? ToDoTasks => Tasks?.Where(t => t.State == "ToDo");
    [NotMapped]
    public IEnumerable<TaskEntity>? InProgressTasks => Tasks?.Where(t => t.State == "InProgress");
    [NotMapped]
    public IEnumerable<TaskEntity>? OnCheckingTasks => Tasks?.Where(t => t.State == "OnChecking");
    [NotMapped]
    public IEnumerable<TaskEntity>? DoneTasks => Tasks?.Where(t => t.State == "Done");
}