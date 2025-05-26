using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;

public class PlannerConfiguration: IEntityTypeConfiguration<PlannerEntity>
{
    public void Configure(EntityTypeBuilder<PlannerEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Planners)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Team)
            .WithMany(t => t.Planners)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Planner)
            .HasForeignKey(t => t.PlannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}