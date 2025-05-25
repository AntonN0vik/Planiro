using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;

public class PlanerConfiguration
{
    public void Configure(EntityTypeBuilder<PlannerEntity> builder)
    {
        builder.HasKey(p => p.Id);

        // Связь с User (1 Planer -> 1 User, 1 User -> Many Planners)
        builder.HasOne(p => p.User)
            .WithMany(u => u.Planners)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь с Team (1 Planer -> 1 Team, 1 Team -> Many Planners)
        builder.HasOne(p => p.Team)
            .WithMany(t => t.Planners)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь с Tasks (1 Planer -> Many Tasks)
        builder.HasMany(p => p.ToDoTasks)
            .WithOne(t => t.Planner)
            .HasForeignKey(t => t.PlannerId);

        builder.HasMany(p => p.InProgressTasks)
            .WithOne(t => t.Planner)
            .HasForeignKey(t => t.PlannerId);

        builder.HasMany(p => p.OnCheckingTasks)
            .WithOne(t => t.Planner)
            .HasForeignKey(t => t.PlannerId);

        builder.HasMany(p => p.DoneTasks)
            .WithOne(t => t.Planner)
            .HasForeignKey(t => t.PlannerId);
    }
}

    