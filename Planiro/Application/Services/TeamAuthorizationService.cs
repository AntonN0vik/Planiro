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
        var result = _userRepository.GetUserByNameAsync(createTeamRequest.Username).Result;
        if (result == null) throw new ArgumentException("Invalid username");
        var teamleadId = result.Id;
        _teamRepository.SaveTeamAsync(new Team(teamId, joinCode, new List<Guid>(), teamleadId));
    }

    public void AuthorizeTeam(JoinTeamRequest joinRequest)
    {
        var username = joinRequest.Username;
        var joinCode = joinRequest.Code;

        if (!_teamRepository.IsJoinCodeValidAsync(joinCode).Result)
            throw new ArgumentException("Invalid joinCode");
        
        var user = _userRepository.GetUserByNameAsync(username).Result;
        if (user != null) 
            _teamRepository.AddUserAsync(user, joinCode);
    }

    public Task<string> GetJoinCode(CreateTeamRequest createTeamRequest)
    {
        var result = _userRepository.GetUserByNameAsync(createTeamRequest.Username).Result;
        if (result == null) throw new ArgumentException("Can't find team with current joinCode");
        var userId = result.Id;
        return _teamRepository.GetJoinCodeAsync(userId);
    }
}