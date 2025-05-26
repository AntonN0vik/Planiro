using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<TeamEntity>
{
    public void Configure(EntityTypeBuilder<TeamEntity> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.HasIndex(t => t.JoinCode).IsUnique();
        
        builder.HasMany(t => t.Users)
            .WithMany(u => u.Teams)
            .UsingEntity(j => j.ToTable("TeamUsers"));

        builder.HasOne(t => t.Teamlead)
            .WithMany()
            .HasForeignKey(t => t.TeamleadId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(t => t.Planners)
            .WithOne(p => p.Team)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}