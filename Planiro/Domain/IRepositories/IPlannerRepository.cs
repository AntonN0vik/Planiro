
namespace Planiro.Domain.IRepositories;

public interface IPlannerRepository
{
    public Task CreatePlanner(string joinCode,  string userName, Guid plannerId);
}