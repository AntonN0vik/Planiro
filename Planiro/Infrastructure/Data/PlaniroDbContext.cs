using Planiro.Infrastructure.Data.Entities;

namespace Planiro.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

public class PlaniroDbContext : DbContext
{
    public DbSet<UserEntity>? Users { get; set; }
    public DbSet<TeamEntity>? Teams { get; set; }
    public DbSet<TaskEntity>? Tasks { get; set; }
    
    public DbSet<PlannerEntity>? Planners { get; set; }
    
    public PlaniroDbContext(DbContextOptions<PlaniroDbContext> options): base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlaniroDbContext).Assembly);
    }
}