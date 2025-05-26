namespace Planiro.Application.Services;
using Planiro.Domain.IRepositories;
public class PlannerService(IPlannerRepository plannerRepository, IUserRepository userRepository, 
    ITeamRepository teamRepository)
{
    public async Task CreatePlanner(string joinCode, string userName)
    {
        var plannerId = Guid.NewGuid();
        var user = await userRepository.GetUserByUserNameAsync(userName);
        var teamId = await teamRepository.GetTeamIdByJoinCodeAsync(joinCode);
        if (await plannerRepository.IsPlannerExists(user.Id, teamId))
            return;
        await plannerRepository.CreatePlanner(user.Id, teamId, plannerId);
    }
}