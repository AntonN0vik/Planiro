﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.HasIndex(t => t.State);
        builder.HasIndex(t => t.Deadline);
        builder.HasIndex(t => t.Title);
        builder.HasIndex(t => new { t.PlannerId, t.State });
        
        builder.HasOne(t => t.Planner)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.PlannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}