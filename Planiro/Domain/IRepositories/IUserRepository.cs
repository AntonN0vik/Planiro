using Planiro.Domain.Entities;
using Task = Planiro.Domain.Entities.Task;

namespace Planiro.Domain.IRepositories;


public interface IUserRepository
{
    public bool IsUsernameExist(string username);
    public bool CheckPassword(string username, string password);
    public User GetUserByName(string username);
    
    public void SaveUser(User user);
    
    public void SaveTeams(ICollection<Team> teams);
    
    public void SaveTasks(ICollection<Task> tasks);
    
    public void AddTeam(Team team);
    
    public void RemoveTeam(Team team);
    
    public void AddTask(Task task);
    
    public void RemoveTask(Task task);
}