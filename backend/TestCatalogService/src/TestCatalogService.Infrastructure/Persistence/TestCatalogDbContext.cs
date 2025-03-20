using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.general_format;
using TestCatalogService.Domain.TestAggregate.tier_list_format;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Domain.TestTagAggregate;

namespace TestCatalogService.Infrastructure.Persistence;

public class TestCatalogDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public DbSet<AppUser> AppUsers { get; set; } = null!;
    public DbSet<BaseTest> BaseTests { get; set; } = null!;
    public DbSet<GeneralFormatTest> GeneralFormatTests { get; set; } = null!;
    public DbSet<TestTag> TestTags { get; set; } = null!;
    public DbSet<TestComment> TestComments { get; set; } = null!;
    public DbSet<TierListFormatTest> TierListFormatTests { get; set; }= null!;

    public TestCatalogDbContext(DbContextOptions<TestCatalogDbContext> options, IPublisher publisher) : base(options) {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var domainEvents = ChangeTracker.Entries<IAggregateRoot>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();
        await PublishDomainEvents(domainEvents);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents) {
        foreach (var domainEvent in domainEvents) {
            await _publisher.Publish(domainEvent);
        }
    }
}