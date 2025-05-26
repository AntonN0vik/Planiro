
using Planiro.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Domain.IRepositories;

public interface IPlannerRepository
{
    public Task CreatePlanner(Guid teamId, Guid userId, Guid plannerId);
}