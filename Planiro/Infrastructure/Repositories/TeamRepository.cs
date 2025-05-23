using Microsoft.EntityFrameworkCore;
using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Planiro.Infrastructure.Data.Configurations;
using Planiro.Infrastructure.Data.Entities;
using Task = System.Threading.Tasks.Task;

namespace Planiro.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
	private readonly PlaniroDbContext _dbContext;

	public TeamRepository(PlaniroDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	
	public async Task<bool> IsJoinCodeValidAsync(string joinCode)
	{
		return await _dbContext.Teams!
			.AnyAsync(x => x.JoinCode == joinCode);
	}
	
	public async Task<ICollection<User>> GetUsersAsync(string joinCode)
	{
		var teamEntity = await _dbContext.Teams!
        .Include(t => t.Users)
        .FirstOrDefaultAsync(t => t.JoinCode == joinCode);

		if (teamEntity == null || teamEntity.Users == null)
			return new List<User>();
    
		return teamEntity.Users.Select(UserRepository.MapUserToDomain).ToList();
	}
	
	public async Task<string?> GetJoinCodeAsync(Guid teamleadId)
	{
		var team = await _dbContext.Teams!
			.FirstOrDefaultAsync(t => t.TeamleadId == teamleadId);

		return team?.JoinCode;
	}
	
	public async Task<User?> GetTeamleadByIdAsync(string joinCode)
	{
		var teamEntity = await _dbContext.Teams!
			.FirstOrDefaultAsync(t => t.JoinCode == joinCode);

		if (teamEntity == null)
			return null;

		var leadEntity = await _dbContext.Users!
			.FirstOrDefaultAsync(u => u.Id == teamEntity.TeamleadId);

		if (leadEntity == null)
			return null;

		return UserRepository.MapUserToDomain(leadEntity);
	}

	public async Task SaveTeamAsync(Team team)
	{
		var existingTeam = await _dbContext.Teams!
			.FirstOrDefaultAsync(t => t.Id == team.Id);

		if (existingTeam == null)
		{
			var entity = MapTeamToEntity(team);
			_dbContext.Teams!.Add(entity);
		}
		else
		{
			existingTeam.JoinCode = team.JoinCode;
			existingTeam.TeamleadId = team.TeamleadId;
		}

		await _dbContext.SaveChangesAsync();
	}
	
	public async Task AddUserAsync(User user, string joinCode)
	{
		var team = await _dbContext.Teams!
			.Include(t => t.Users)
			.FirstOrDefaultAsync(t => t.JoinCode == joinCode);

		if (team == null)
			throw new InvalidOperationException("Команда не найдена");

		var userEntity = await _dbContext.Users!
			.FirstOrDefaultAsync(u => u.Id == user.Id);

		if (userEntity == null)
			throw new InvalidOperationException("Пользователь не найден");

		if (team.Users == null)
			team.Users = new List<UserEntity>();

		if (!team.Users.Any(u => u.Id == userEntity.Id))
		{
			team.Users.Add(userEntity);
			await _dbContext.SaveChangesAsync();
		}
	}
	public async Task RemoveUserAsync(User user, string joinCode)
	{
		var team = await _dbContext.Teams!
			.Include(t => t.Users)
			.FirstOrDefaultAsync(t => t.JoinCode == joinCode);

		if (team == null)
			throw new InvalidOperationException("Команда не найдена");

		if (team.Users == null)
			return;

		var userEntity = team.Users.FirstOrDefault(u => u.Id == user.Id);

		if (userEntity != null)
		{
			team.Users.Remove(userEntity);
			await _dbContext.SaveChangesAsync();
		}
	}
	
	private static TeamEntity MapTeamToEntity(Team team)
	{
		return new TeamEntity
		{
			Id = team.Id,
			JoinCode = team.JoinCode,
			TeamleadId = team.TeamleadId
			// Users устанавливаются отдельно
		};
	}

	private static Team MapTeamToDomain(TeamEntity entity)
	{
		return new Team(
			entity.Id,
			entity.JoinCode,
			entity.Users?.Select(u => u.Id).ToList(),
			entity.TeamleadId
			);
	}
}