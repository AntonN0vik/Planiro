using Planiro.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Domain.IRepositories;

public interface IUserRepository
{
	Task<User> GetUserByIdAsync(Guid userId);
	
	Task<User> GetUserByUserNameAsync(string userName);

	Task<bool> IsUsernameExistAsync(string userName);
	
	Task<bool> CheckPasswordAsync(string userName, string password);
	
	Task SaveUserAsync(User user, string password);
}