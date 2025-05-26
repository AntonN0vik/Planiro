using Planiro.Domain.IRepositories;

namespace Planiro.Infrastructure.Repositories;

public class PlannerRepository : IPlannerRepository
{
    public Task CreatePlanner(string joinCode, string userName, Guid plannerId)
    {
        throw new NotImplementedException();
    }
}