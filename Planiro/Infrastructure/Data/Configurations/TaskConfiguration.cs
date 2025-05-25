using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(t => t.Id);
        
        // Связь с Planer (Many Tasks -> 1 Planer)
        builder.HasOne(t => t.Planner)
            .WithMany(p => p.ToDoTasks) // Выберите нужную коллекцию
            .HasForeignKey(t => t.PlannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}