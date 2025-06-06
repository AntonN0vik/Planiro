﻿using Planiro.Domain.Entities;
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
        var plannerId = Guid.NewGuid();
        var joinCode = RandomCodeGenerator.GenerateCode();
        var result = await _userRepository.GetUserByUserNameAsync(createTeamRequest.Username);
        if (result == null) throw new ArgumentException("Invalid username");
        var teamleadId = result.Id;
        var plannersId = new List<Guid> { plannerId };
        await _teamRepository.SaveTeamAsync(new Team(teamId,
            joinCode, new List<Guid>(), teamleadId, plannersId));
        return joinCode;
    }

    public async Task<Guid> AuthorizeTeamAndGetId(JoinTeamRequest joinRequest)
    {
        var username = joinRequest.Username;
        var joinCode = joinRequest.Code;
        var teamId = await _teamRepository.GetTeamIdByJoinCodeAsync(joinCode);

        var result = await _teamRepository.IsJoinCodeValidAsync(joinCode);
        if (!result)
            throw new ArgumentException("Invalid joinCode");

        var user = await _userRepository.GetUserByUserNameAsync(username);
        if (user == null)
            throw new ArgumentException($"User '{joinRequest.Username}' not found", nameof(joinRequest.Username));

        await _teamRepository.AddUserAsync(user, teamId);
        return teamId;
    }

    // public async Task<string> GetJoinCode(CreateTeamRequest createTeamRequest)
    // {
    //     var result = await _userRepository.GetUserByNameAsync(createTeamRequest.Username);
    //     if (result == null) throw new ArgumentException("Can't find team with current joinCode");
    //     var userId = result.Id;
    //     return await _teamRepository.GetJoinCodeAsync(userId);
    // }
}