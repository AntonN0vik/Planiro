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

        return new User(entity.Id, entity.FirstName ?? "", entity.LastName ?? "", entity.UserName ?? "")
        {
            Teams = entity.Teams?.Select(t => t.Id).ToList(),
            Planners = entity.Tasks?.Select(t => t.Id).ToList()
        };
    }
    
    public async Task SaveUserAsync(User user, string password)
    {
        var entity = MapToEntity(user, password);
        await _dbContext.Users!.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    
    public Task SaveTeamsAsync(ICollection<Team> teams)
    {
        throw new NotImplementedException();
    }
    
    public Task SaveTasksAsync(ICollection<TaskDomain> tasks)
    {
        throw new NotImplementedException();
    }
    
    public Task AddTeamAsync(Team team)
    {
        throw new NotImplementedException();
    }
    
    public Task RemoveTeamAsync(Team team)
    {
        throw new NotImplementedException();
    }
    
    public Task AddTaskAsync(Task task)
    {
        throw new NotImplementedException();
    }
    
    public Task RemoveTaskAsync(Task task)
    {
        throw new NotImplementedException();
    }
    
    private static User MapToDomain(UserEntity entity)
    {
        return new User(entity.Id, entity.FirstName!, entity.LastName!, entity.UserName!)
        {
            Teams = entity.Teams?.Select(t => t.Id).ToList(),
            Planners = entity.Tasks?.Select(t => t.Id).ToList()
        };
    }

    private static UserEntity MapToEntity(User user, string passwordHash)
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
