using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Planiro.Models;

namespace Planiro.Application.Services;

public class TeamAuthorizationService
{
    public static void RegisterTeam(ITeamRepository teamRepository,
        IUserRepository userRepository,
        CreateTeamRequest createTeamRequest)
    {
        var teamId = Guid.NewGuid();
        var joinCode = RandomCodeGenerator.GenerateCode();

        var teamleadId = userRepository.GetUserByName(createTeamRequest.Username).Id;
        teamRepository.SaveTeam(new Team(teamId, joinCode, default, teamleadId));
    }

    public static void AuthorizeTeam(ITeamRepository teamRepository, 
        IUserRepository userRepository,
        JoinTeamRequest joinRequest)
    {
        var username = joinRequest.Username;
        var joinCode = joinRequest.Code;

        if (!teamRepository.IsJoinCodeValid(joinCode))
            throw new ArgumentException("Invalid joinCode");
        
        var user = userRepository.GetUserByName(username);
        teamRepository.AddUser(user,  joinCode);
    }

    public static string GetJoinCode(ITeamRepository teamRepository,
        IUserRepository userRepository,
        CreateTeamRequest createTeamRequest)
    {
        var userId = userRepository.GetUserByName(createTeamRequest.Username).Id;
        return teamRepository.GetJoinCode(userId);
    }
}