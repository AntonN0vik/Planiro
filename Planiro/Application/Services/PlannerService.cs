namespace Planiro.Application.Services;
using Planiro.Domain.IRepositories;
public class PlannerService(IPlannerRepository repository, IUserRepository userRepository, 
    ITeamRepository teamRepository)
{
    public async Task CreatePlanner(string joinCode, string userName)
    {
        var plannerId = Guid.NewGuid();
        var user = await userRepository.GetUserByUserNameAsync(userName);
        var teamId = await teamRepository.GetTeamIdByJoinCodeAsync(joinCode);
        await repository.CreatePlanner(user.Id, teamId, plannerId);
    }
}