using Microsoft.EntityFrameworkCore;
using Planiro.Domain.Entities;
using Planiro.Domain.IRepositories;
using Planiro.Infrastructure.Data.Configurations;
using Planiro.Infrastructure.Data.Entities;

using Task = System.Threading.Tasks.Task;
using TaskDomain = Planiro.Domain.Entities.Task;

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
	
	public async Task<ICollection<User>> GetUsersAsync(Guid teamId)
	{
		var entity = await _dbContext.Teams!
			.Include(t => t.Users)
			.FirstOrDefaultAsync(t => t.Id == teamId);
		
		return entity.Users.Select(Mappers.MapUserToDomain).ToList();
	}
	
	public async Task<string?> GetJoinCodeAsync(Guid teamleadId)
	{
		var team = await _dbContext.Teams!
			.FirstOrDefaultAsync(t => t.TeamleadId == teamleadId);

		return team?.JoinCode;
	}

	public async Task<Guid> GetTeamIdByJoinCodeAsync(string joinCode)
	{
		var team = await _dbContext.Teams!
			.FirstOrDefaultAsync(t => t.JoinCode == joinCode);

		if (team == null)
			throw new InvalidOperationException("Команда с указанным кодом не найдена");

		return team.Id;
	}

	public async Task<Guid> GetPlannerIdAsync(Guid teamId, Guid userId)
	{
		var planner = await _dbContext.Planners!
			.FirstOrDefaultAsync(p => p.TeamId == teamId && p.UserId == userId);

		if (planner == null)
			throw new InvalidOperationException("Планировщик не найден для указанного пользователя в команде");

		return planner.Id;
	}

	public async Task<ICollection<TaskDomain>> GetTasksByTeamIdAsync(Guid teamId)
	{
		var tasks = await _dbContext.Tasks!
			.Include(t => t.Planner)
			.Where(t => t.Planner.TeamId == teamId)
			.ToListAsync();

		return tasks.Select(Mappers.MapTaskToDomain).ToList();
	}

	public  async Task<User?> GetTeamleadByIdAsync(Guid teamId)
	{
		var team = await _dbContext.Teams!
			.FirstOrDefaultAsync(t => t.Id == teamId);

		if (team == null)
			return null;

		var leadEntity = await _dbContext.Users!
			.FirstOrDefaultAsync(u => u.Id == team.TeamleadId);

		if (leadEntity == null)
			return null;

		return Mappers.MapUserToDomain(leadEntity);
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

		return Mappers.MapUserToDomain(leadEntity);
	}

	public async Task SaveTeamAsync(Team team)
	{
		var existingTeam = await _dbContext.Teams!
			.FirstOrDefaultAsync(t => t.Id == team.Id);

		if (existingTeam == null)
		{
			var entity = Mappers.MapTeamToEntity(team);
			_dbContext.Teams!.Add(entity);
		}
		else
		{
			existingTeam.JoinCode = team.JoinCode;
			existingTeam.TeamleadId = team.TeamleadId;
		}

		await _dbContext.SaveChangesAsync();
	}
	
	public async Task AddUserAsync(User user, Guid teamId)
	{
		var team = await _dbContext.Teams!
			.Include(t => t.Users)
			.FirstOrDefaultAsync(t => t.Id == teamId);

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
	
	public async Task RemoveUserAsync(User user, Guid teamId)
	{
		var team = await _dbContext.Teams!
			.Include(t => t.Users)
			.FirstOrDefaultAsync(t => t.Id == teamId);

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
}