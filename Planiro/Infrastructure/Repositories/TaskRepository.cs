using Microsoft.EntityFrameworkCore;
using Planiro.Domain.IRepositories;
using Planiro.Infrastructure.Data.Configurations;
using Planiro.Infrastructure.Data.Entities;

using Task = System.Threading.Tasks.Task;
using TaskDomain = Planiro.Domain.Entities.Task;

namespace Planiro.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
	private readonly PlaniroDbContext _dbContext;

	public TaskRepository(PlaniroDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	
	private static TaskEntity MapToEntity(Domain.Entities.Task task)
	{
		return new TaskEntity
		{
			Id = task.Id,
			Title = task.Title,
			Description = task.Description,
			Deadline = task.Deadline,
			IsApproved = task.IsApproved,
			State = task.State.ToString(),
			UserId = task.PlannerId
			// TeamId можно добавить, если понадобится
		};
	}

	private static Domain.Entities.Task MapToDomain(TaskEntity entity)
	{
		Enum.TryParse(entity.State, out Domain.Entities.Task.States state);

		return new Domain.Entities.Task
		{
			Id = entity.Id,
			Title = entity.Title,
			Description = entity.Description,
			Deadline = entity.Deadline,
			IsApproved = entity.IsApproved,
			State = state,
			PlannerId = entity.UserId
		};
	}

	public async Task<ICollection<TaskDomain>> GetTaskByUserIdAsync(Guid userId)
	{
		var entities = await _dbContext.Tasks!
			.Where(t => t.UserId == userId)
			.ToListAsync();

		return entities.Select(MapToDomain).ToList();
	}
	
	public async Task<ICollection<TaskDomain>> GetTaskByTeamIdAsync(Guid teamId)
	{
		var entities = await _dbContext.Tasks!
			.Where(t => t.TeamId == teamId)
			.ToListAsync();

		return entities.Select(MapToDomain).ToList();
	}
	
	public async Task SaveTaskAsync(TaskDomain task)
	{
		var existing = await _dbContext.Tasks.FindAsync(task.Id);
		if (existing != null)
		{
			existing.Title = task.Title;
			existing.Description = task.Description;
			existing.Deadline = task.Deadline;
			existing.IsApproved = task.IsApproved;
			existing.State = task.State.ToString();
			existing.UserId = task.PlannerId;
		}
		else
		{
			var entity = MapToEntity(task);
			await _dbContext.Tasks.AddAsync(entity);
		}
		await _dbContext.SaveChangesAsync();
	}

	public async Task RemoveTaskAsync(TaskDomain task)
	{
		var existing = await _dbContext.Tasks.FindAsync(task.Id);
		if (existing != null)
		{
			_dbContext.Tasks!.Remove(existing);
			await _dbContext.SaveChangesAsync();
		}
	}
}