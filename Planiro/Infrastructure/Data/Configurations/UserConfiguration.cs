using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        // Связь с Planer (1 User -> Many Planers)
        builder.HasMany(u => u.Planners)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь Many-to-Many с Team
        builder.HasMany(u => u.Teams)
            .WithMany(t => t.Users)
            .UsingEntity(j => j.ToTable("TeamUsers"));
        
    }
}