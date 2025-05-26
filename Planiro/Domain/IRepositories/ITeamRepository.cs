using Planiro.Domain.Entities;

using Task = System.Threading.Tasks.Task;
using TaskDomain = Planiro.Domain.Entities.Task;

namespace Planiro.Domain.IRepositories;

public interface ITeamRepository
{
    Task<bool> IsJoinCodeValidAsync(string joinCode);

    Task<ICollection<User>> GetUsersAsync(Guid teamId);
    
    Task<string> GetJoinCodeAsync(Guid teamId);
    
    Task<Guid> GetTeamIdByJoinCodeAsync(string joinCode);
    
    Task<Guid> GetPlannerIdAsync(Guid teamId, Guid userId);
    //идет в бд команд - находит нужную по id -> берёт коллекцию планеров ->
    // в этих планерах находит нужный по id юзера -> возвращает id planner  
    
    Task<ICollection<TaskDomain>> GetTasksByTeamIdAsync(Guid teamId); 
    
    Task<User> GetTeamleadByIdAsync(Guid teamId);
    // ищет в бд команду по коду вступления, берет id тимы,
    // идет в таблицу юзеров и находит тимлида

    Task SaveTeamAsync(Team team);
    
    Task AddUserAsync(User user, Guid teamId);
    
    Task RemoveUserAsync(User user, Guid teamId);
}
