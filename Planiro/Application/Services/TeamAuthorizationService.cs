using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Planiro.Models;
using Task = System.Threading.Tasks.Task;

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

    public async Task<string> RegisterTeam(CreateTeamRequest createTeamRequest)
    {
        var teamId = Guid.NewGuid();
        var joinCode = RandomCodeGenerator.GenerateCode();
        var result = await _userRepository.GetUserByNameAsync(createTeamRequest.Username);
        if (result == null) throw new ArgumentException("Invalid username");
        var teamleadId = result.Id;
        await _teamRepository.SaveTeamAsync(new Team(teamId, joinCode, new List<Guid>(), teamleadId));
        return joinCode;
    }

    public async Task AuthorizeTeam(JoinTeamRequest joinRequest)
    {
        var username = joinRequest.Username;
        var joinCode = joinRequest.Code;

        var result = await _teamRepository.IsJoinCodeValidAsync(joinCode);
        if (!result)
            throw new ArgumentException("Invalid joinCode");
        
        var user = await _userRepository.GetUserByNameAsync(username);
        if (user == null)
            throw new ArgumentException($"User '{joinRequest.Username}' not found", nameof(joinRequest.Username));

        await _teamRepository.AddUserAsync(user, joinRequest.Code);
    }

    // public async Task<string> GetJoinCode(CreateTeamRequest createTeamRequest)
    // {
    //     var result = await _userRepository.GetUserByNameAsync(createTeamRequest.Username);
    //     if (result == null) throw new ArgumentException("Can't find team with current joinCode");
    //     var userId = result.Id;
    //     return await _teamRepository.GetJoinCodeAsync(userId);
    // }
}