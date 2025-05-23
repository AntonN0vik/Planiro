using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<TeamEntity>
{
    public void Configure(EntityTypeBuilder<TeamEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.Users)
            .WithMany(x => x.Teams)
            .UsingEntity(j => j.ToTable("TeamUsers"));
        
    }
}