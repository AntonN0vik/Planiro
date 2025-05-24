using Planiro.Domain.Entities;

namespace Planiro.Domain.IRepositories;

using Planiro.Domain;

public interface ITeamRepository
{
    public bool IsJoinCodeValid(string joinCode);

    public ICollection<User> GetUsers(string joinCode);
    
    public string GetJoinCode(Guid teamleadId);
    
    public User GetTeamleadById(string joinCode); 
    // ищет в бд команду по коду вступления, берет id тимлида,
    // идет в таблицу юзеров и находит тимлида
    
    public void SaveTeam(Team team);
    
    public void AddUser(User user, string joinCode);
    
    public void RemoveUser(User user,  string joinCode);
}