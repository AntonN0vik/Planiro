using Planiro.Domain.IRepositories;
using Planiro.Domain.Entities;
using Task = System.Threading.Tasks.Task;
using TaskDomain =  Planiro.Domain.Entities.Task;
namespace Planiro.Application.Services;

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

        public async Task<IEnumerable<User>> GetTeamMembersAsync(Guid teamId)
        {
            return await _teamRepository.GetUsersAsync(teamId);
        }

        public async Task RemoveMemberAsync(Guid teamId, Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            await _teamRepository.RemoveUserAsync(user, teamId);
            await _teamRepository.AddUserAsync(user, userId);
        }

        public async Task<Guid> GetPlannerIdAsync(Guid teamId, Guid userId)
        {
            return await _teamRepository.GetPlannerIdAsync(teamId, userId);
        }

        public async Task<ICollection<TaskDomain>> GetTasksByTeamIdAsync(Guid teamId)
        {
            return await _teamRepository.GetTasksByTeamIdAsync(teamId);
        }

    }