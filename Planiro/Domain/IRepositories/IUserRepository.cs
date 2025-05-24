using Planiro.Domain.Entities;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Planiro.Domain.Entities.Task;

namespace Planiro.Domain.IRepositories;


public interface IUserRepository
{
    Task<bool> IsUsernameExistAsync(string username);
    Task<bool> CheckPasswordAsync(string username, string password);
    Task<User?> GetUserByNameAsync(string username);
    Task SaveUserAsync(User user, string password);
    Task SaveTeamsAsync(ICollection<Team> teams);
    Task SaveTasksAsync(ICollection<Task> tasks);
    Task AddTeamAsync(Team team);
    Task RemoveTeamAsync(Team team);
    Task AddTaskAsync(Task task);
    Task RemoveTaskAsync(Task task);
}