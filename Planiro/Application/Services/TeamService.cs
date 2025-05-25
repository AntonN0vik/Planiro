using Planiro.Domain.IRepositories;
using Planiro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Application.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public TeamService(
            ITeamRepository teamRepository,
            IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<string> CreateTeamAsync(Team team)
        {
            await _teamRepository.SaveTeamAsync(team);
            return team.JoinCode;
        }

        public async Task JoinTeamAsync(string joinCode, Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            await _teamRepository.AddUserAsync(user, userId);
        }

        public async Task<IEnumerable<User>> GetTeamMembersAsync(string joinCode)
        {
            return await _teamRepository.GetUsersAsync(joinCode);
        }

        public async Task RemoveMemberAsync(string joinCode, Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            await _teamRepository.RemoveUserAsync(user, userId);
        }
    }
}