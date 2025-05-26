namespace Planiro.Application.Services;
using Planiro.Domain.IRepositories;
public class PlannerService(IPlannerRepository repository)
{
    public async Task CreatePlanner(string joinCode, string userName)
    {
        var plannerId = Guid.NewGuid();
        await repository.CreatePlanner(joinCode, userName, plannerId);
    }
}