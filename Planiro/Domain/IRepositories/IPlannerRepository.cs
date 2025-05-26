
using Planiro.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Domain.IRepositories;

public interface IPlannerRepository
{
    public Task CreatePlanner(Guid userId, Guid teamId, Guid plannerId);
    public Task<bool> IsPlannerExists(Guid userId, Guid teamId);
}