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
	
	public async Task<TaskDomain> GetTaskByIdAsync(Guid taskId)
	{
		var entity = await _dbContext.Tasks!
			.FirstOrDefaultAsync(u => u.Id == taskId);
		return Mappers.MapTaskToDomain(entity);
	}
	
	public async Task<ICollection<TaskDomain>> GetTaskByUserIdAsync(Guid plannerId)
	{
		var entities = await _dbContext.Tasks!
			.Where(t => t.PlannerId == plannerId)
			.ToListAsync();

		return entities.Select(Mappers.MapTaskToDomain).ToList();
	}
	
	public async Task SaveTaskAsync(TaskDomain task)
	{
		var entity = await _dbContext.Tasks
			.FirstOrDefaultAsync(t => t.Id == task.Id);

		if (entity == null)
		{
			var newEntity = Mappers.MapTaskToEntity(task);
			await _dbContext.Tasks.AddAsync(newEntity);
		}
		else
		{
			entity.Title = task.Title;
			entity.Description = task.Description;
			entity.Deadline = task.Deadline;
			entity.State = task.State.ToString();
			entity.IsApproved = task.IsApproved;
			entity.PlannerId = task.PlannerId;
		}

		await _dbContext.SaveChangesAsync();
	}
	
	public async Task RemoveTaskAsync(TaskDomain task)
	{
		var entity = await _dbContext.Tasks
			.FirstOrDefaultAsync(t => t.Id == task.Id);

		if (entity != null)
		{
			_dbContext.Tasks.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}
	}
}