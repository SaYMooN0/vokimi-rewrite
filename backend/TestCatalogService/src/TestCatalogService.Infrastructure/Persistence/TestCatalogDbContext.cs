using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel.Common;
using SharedKernel.Common.domain;
using System.Reflection;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Infrastructure.Persistence;

public class TestCatalogDbContext : DbContext
{
    private readonly IPublisher _publisher;


    public DbSet<BaseTest> BaseTests { get; set; } = null!;
    public TestCatalogDbContext(DbContextOptions options, IPublisher publisher) : base(options) {
        _publisher = publisher;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
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

