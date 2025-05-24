using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Planiro.Models;

namespace Planiro.Application.Services;

public class TeamAuthorizationService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;

    public TeamAuthorizationService(
        ITeamRepository teamRepository,
        IUserRepository userRepository)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
    }

    public void RegisterTeam(CreateTeamRequest createTeamRequest)
    {
        var teamId = Guid.NewGuid();
        var joinCode = RandomCodeGenerator.GenerateCode();
        var teamleadId = _userRepository.GetUserByName(createTeamRequest.Username).Id;
        
        _teamRepository.SaveTeam(new Team(teamId, joinCode, default, teamleadId));
    }

    public void AuthorizeTeam(JoinTeamRequest joinRequest)
    {
        var username = joinRequest.Username;
        var joinCode = joinRequest.Code;

        if (!_teamRepository.IsJoinCodeValid(joinCode))
            throw new ArgumentException("Invalid joinCode");
        
        var user = _userRepository.GetUserByName(username);
        _teamRepository.AddUser(user, joinCode);
    }

    public string GetJoinCode(CreateTeamRequest createTeamRequest)
    {
        var userId = _userRepository.GetUserByName(createTeamRequest.Username).Id;
        return _teamRepository.GetJoinCode(userId);
    }
}