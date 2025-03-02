using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using TestManagingService.Domain.AppUserAggregate;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.general_format;
using TestManagingService.Domain.TestFeedbackRecordAggregate;
using TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestManagingService.Infrastructure.Persistence;

public class TestManagingDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public DbSet<AppUser> AppUsers { get; set; } = null!;
    //tests
    public DbSet<BaseTest> BaseTests { get; set; } = null!;
    public DbSet<GeneralFormatTest> GeneralFormatTests { get; set; } = null!;

    //feedback records
    public DbSet<BaseTestFeedbackRecord> BaseTestFeedbackRecords { get; set; } = null!;
    public DbSet<GeneralTestFeedbackRecord> GeneralTestFeedbackRecords { get; set; } = null!;
    

    public TestManagingDbContext(DbContextOptions<TestManagingDbContext> options, IPublisher publisher) :
        base(options) {
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