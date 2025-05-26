using Microsoft.EntityFrameworkCore;
using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Planiro.Infrastructure.Data.Configurations;
using Planiro.Infrastructure.Data.Entities;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Infrastructure.Repositories;

public class PlannerRepository : IPlannerRepository
{
    private readonly PlaniroDbContext _dbContext;

    public PlannerRepository(PlaniroDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreatePlanner(Guid userId, Guid teamId, Guid plannerId)
    {
        var planner = new PlannerEntity
        {
            Id = plannerId,
            UserId = userId,
            TeamId = teamId,
            Tasks = new List<TaskEntity>()
        };

        _dbContext.Planners.Add(planner);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> IsPlannerExists(Guid userId, Guid teamId)
    {
        var entity = await _dbContext.Planners!
            .FirstOrDefaultAsync(p => p.UserId == userId && p.TeamId == teamId);
    
        return entity != null;
    }
}