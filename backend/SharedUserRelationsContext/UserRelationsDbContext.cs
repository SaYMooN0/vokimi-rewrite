using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedUserRelationsContext.Entities;

namespace SharedUserRelationsContext;

public class UserRelationsDbContext : DbContext
{
    public DbSet<UserFollowing> UserFollowings { get; set; } = null!;
    public DbSet<UserRelationsAppUser> AppUsers { get; set; } = null!;
    public UserRelationsDbContext(DbContextOptions<UserRelationsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}