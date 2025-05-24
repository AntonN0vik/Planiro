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


    public async Task<bool> IsUsernameExistAsync(string username)
    {
        return await _dbContext.Users!
            .AnyAsync(u => u.UserName == username);
    }
    
    public async Task<bool> CheckPasswordAsync(string username, string password)
    {
        var user = await _dbContext.Users!
            .FirstOrDefaultAsync(u => u.UserName == username);
        return user != null && user.PasswordHash == password;
    }

    
    public async Task<User?> GetUserByNameAsync(string username)
    {
        var entity = await _dbContext.Users
            .Include(u => u.Teams)
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (entity == null)
            return null;

        return MapUserToDomain(entity);
    }
    
    public async Task SaveUserAsync(User user, string password)
    {
        var entity = MapUserToEntity(user, password);
        await _dbContext.Users!.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    
    public static User MapUserToDomain(UserEntity entity)
    {
        return new User(
            entity.Id,
            entity.FirstName ?? "",
            entity.LastName ?? "",
            entity.UserName ?? "",
            entity.Teams?.Select(t => t.Id).ToList(),
            entity.Tasks?.Select(t => t.Id).ToList()
            );
    }

    public static UserEntity MapUserToEntity(User user, string passwordHash)
    {
        return new UserEntity
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            PasswordHash = passwordHash
            // Teams и Tasks будут устанавливаться отдельно при необходимости
        };
    }
    
}
