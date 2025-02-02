using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.interfaces;
using System.Reflection;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.TestAggregate.scoring_format;

namespace TestCreationService.Infrastructure.Persistence;

public class TestCreationDbContext : DbContext
{
    private readonly IPublisher _publisher;


    public DbSet<AppUser> AppUsers { get; set; } = null!;
    public DbSet<BaseTest> BaseTests { get; set; } = null!;
    public DbSet<GeneralFormatTest> GeneralFormatTests { get; set; } = null!;
    public DbSet<ScoringFormatTest> ScoringFormatTests { get; set; } = null!;
    public DbSet<GeneralTestQuestion> GeneralTestQuestions { get; set; } = null!;
    public TestCreationDbContext(DbContextOptions options, IPublisher publisher) : base(options) {
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
