using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using TestTakingService.Domain.AppUserAggregate;
using TestTakingService.Domain.TestAggregate;
using TestTakingService.Domain.TestAggregate.general_format;
using TestTakingService.Domain.TestTakenRecordAggregate;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test;

namespace TestTakingService.Infrastructure.Persistence;

public class TestTakingDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public DbSet<AppUser> AppUsers { get; set; } = null!;

    //tests
    public DbSet<BaseTest> BaseTests { get; set; } = null!;

    public DbSet<GeneralFormatTest> GeneralFormatTests { get; set; } = null!;

    //test taken records
    public DbSet<BaseTestTakenRecord> BaseTestTakenRecords { get; set; } = null!;

    public DbSet<GeneralTestTakenRecord> GeneralTestTakenRecords { get; set; } = null!;


    public TestTakingDbContext(DbContextOptions<TestTakingDbContext> options, IPublisher publisher) : base(options) {
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