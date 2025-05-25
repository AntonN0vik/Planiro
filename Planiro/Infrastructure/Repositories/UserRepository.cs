using Microsoft.EntityFrameworkCore;
using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Planiro.Infrastructure.Data.Configurations;
using Planiro.Infrastructure.Data.Entities;

using Task = System.Threading.Tasks.Task;
using TaskDomain = Planiro.Domain.Entities.Task;

namespace Planiro.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PlaniroDbContext _dbContext;

    public UserRepository(PlaniroDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        var entity = await _dbContext.Users!
            .Include(u => u.Teams)
            .Include(u => u.Planners)
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        return Mappers.MapUserToDomain(entity);
    }

    public async Task<User> GetUserByUserNameAsync(string userName)
    {
        var entity = await _dbContext.Users!
            .Include(u => u.Teams)
            .Include(u => u.Planners)
            .FirstOrDefaultAsync(u => u.UserName == userName);
        
        return Mappers.MapUserToDomain(entity);
    }
    
    public async Task<bool> IsUsernameExistAsync(string userName)
    {
        return await _dbContext.Users!
            .AnyAsync(u => u.UserName == userName);
    }
    
    public async Task<bool> CheckPasswordAsync(string userName, string password)
    {
        var entity = await _dbContext.Users!
            .FirstOrDefaultAsync(u => u.UserName == userName);
        
        return entity.PasswordHash == password;
    }
    
    public async Task SaveUserAsync(User user, string password)
    {
        var entity = Mappers.MapUserToEntity(user, password);
        await _dbContext.Users!.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
}
