using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Planiro.Infrastructure.Data.Entities;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
	private static TeamEntity MapToEntity(Team team)
	{
		return new TeamEntity
		{
			Id = team.Id,
			JoinCode = team.JoinCode,
			TeamleadId = team.TeamleadId
			// Users устанавливаются отдельно
		};
	}

	private static Team MapToDomain(TeamEntity entity)
	{
		return new Team(
			entity.Id,
			entity.JoinCode,
			entity.Users?.Select(u => u.Id).ToList(),
			entity.TeamleadId
			);
	}
	public Task<bool> IsJoinCodeValidAsync(string joinCode)
	{
		throw new NotImplementedException();
	}
	public Task<ICollection<User>> GetUsersAsync(string joinCode)
	{
		throw new NotImplementedException();
	}
	public Task<string?> GetJoinCodeAsync(Guid teamleadId)
	{
		throw new NotImplementedException();
	}
	public Task<User?> GetTeamleadByIdAsync(string joinCode)
	{
		throw new NotImplementedException();
	}
	public Task SaveTeamAsync(Team team)
	{
		throw new NotImplementedException();
	}
	public Task AddUserAsync(User user, string joinCode)
	{
		throw new NotImplementedException();
	}
	public Task RemoveUserAsync(User user, string joinCode)
	{
		throw new NotImplementedException();
	}
}