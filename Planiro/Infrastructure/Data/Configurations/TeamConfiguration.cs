using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<TeamEntity>
{
    public void Configure(EntityTypeBuilder<TeamEntity> builder)
    {
        builder.HasKey(t => t.Id);

        // Связь Many-to-Many между Team и User
        builder.HasMany(t => t.Users)
            .WithMany(u => u.Teams)
            .UsingEntity(j => j.ToTable("TeamUsers"));

        // Связь с Planer (1 Team -> Many Planers)
        builder.HasMany(t => t.Planers)
            .WithOne(p => p.Team)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}