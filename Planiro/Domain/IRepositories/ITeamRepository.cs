using Planiro.Domain.Entities;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Planiro.Domain.Entities.Task;

namespace Planiro.Domain.IRepositories;

using Planiro.Domain;

public interface ITeamRepository
{
    Task<bool> IsJoinCodeValidAsync(string joinCode);

    Task<ICollection<User>> GetUsersAsync(string joinCode);
    
    Task<string> GetJoinCodeAsync(Guid teamleadId);
    
    Task<User?> GetTeamleadByIdAsync(string joinCode);
    // ищет в бд команду по коду вступления, берет id тимлида,
    // идет в таблицу юзеров и находит тимлида

    Task SaveTeamAsync(Team team);
    
    Task AddUserAsync(User user, string joinCode);
    
    Task RemoveUserAsync(User user, string joinCode);
}
