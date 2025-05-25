using Planiro.Domain.Entities;
using Planiro.Infrastructure.Data.Entities;
using Task = Planiro.Domain.Entities.Task;

namespace Planiro.Infrastructure.Repositories;

public class Mappers
{	
	public static TaskEntity MapTaskToEntity(Task task)
	{
		return new TaskEntity
		{
			Id = task.Id,
			Title = task.Title,
			Description = task.Description,
			Deadline = task.Deadline,
			State = task.State.ToString(),
			IsApproved = task.IsApproved,
			PlannerId = task.PlannerId
		};
	}

	public static Task MapTaskToDomain(TaskEntity entity)
	{
		Task.States state;
		Enum.TryParse(entity.State, out state);

		return new Task
		(
			entity.Id,
			entity.Title,
			entity.Description,
			state,
			entity.Deadline,
			entity.PlannerId,
			entity.IsApproved
		);
	}
	
	public static UserEntity MapUserToEntity(User user, string passwordHash)
	{
		return new UserEntity
		{
			Id = user.Id,
			FirstName = user.FirstName,
			LastName = user.LastName,
			UserName = user.UserName,
			PasswordHash = passwordHash,
			Teams = user.Teams?.Select(id => new TeamEntity { Id = id }).ToList(),
			Planners = user.Planners?.Select(id => new PlannerEntity { Id = id }).ToList(),

		};
	}
	
	public static User MapUserToDomain(UserEntity entity)
	{
		return new User(
			entity.Id,
			entity.FirstName,
			entity.LastName,
			entity.UserName,
			entity.Teams?.Select(t => t.Id).ToList(),
			entity.Planners?.Select(t => t.Id).ToList()
			);
	}
	
	public static TeamEntity MapTeamToEntity(Team team)
	{
		return new TeamEntity
		{
			Id = team.Id,
			JoinCode = team.JoinCode,
			TeamleadId = team.TeamleadId,
			Users = team.Collaborators?.Select(id => new UserEntity { Id = id }).ToList(),
			Planners = team.Planners?.Select(id => new PlannerEntity { Id = id }).ToList()
		};
	}

	public static Team MapTeamToDomain(TeamEntity entity)
	{
		return new Team(
			entity.Id,
			entity.JoinCode,
			entity.Users?.Select(u => u.Id).ToList(),
			entity.TeamleadId,
			entity.Planners?.Select(pl => pl.Id).ToList()
			);
	}
}