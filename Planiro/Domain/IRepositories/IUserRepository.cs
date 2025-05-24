using Planiro.Domain.Entities;
using Task = System.Threading.Tasks.Task;
using TaskDomain = Planiro.Domain.Entities.Task;

namespace Planiro.Domain.IRepositories;


public interface IUserRepository
{
	Task<bool> IsUsernameExistAsync(string username);
	Task<bool> CheckPasswordAsync(string username, string password);
	Task<User?> GetUserByNameAsync(string username);
	Task SaveUserAsync(User user, string password);
	
}