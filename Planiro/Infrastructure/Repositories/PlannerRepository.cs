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
    
    public async Task CreatePlanner(Guid teamId, Guid userId, Guid plannerId)
    {
        var planner = new PlannerEntity
        {
            Id = plannerId,
            UserId = userId,
            TeamId = teamId
        };

        _dbContext.Planners.Add(planner);
        await _dbContext.SaveChangesAsync();
    }
}