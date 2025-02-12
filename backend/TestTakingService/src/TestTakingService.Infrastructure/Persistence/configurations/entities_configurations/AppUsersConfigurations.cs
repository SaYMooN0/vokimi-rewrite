using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using TestTakingService.Domain.AppUserAggregate;
using TestTakingService.Domain.Common;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations;

public class AppUsersConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder.Ignore(x => x.TakenTestIds);
        builder
            .Property<HashSet<TestId>>("_takenTestIds")
            .HasColumnName("TakenTestIds")
            .HasEntityIdsHashSetConversion();

        builder.Ignore(x => x.TestTakenRecordIds);
        builder
            .Property<HashSet<TestTakenRecordId>>("_testTakenRecordIds")
            .HasColumnName("TestTakenRecordIds")
            .HasEntityIdsHashSetConversion();

        builder.Ignore(x => x.FeedbackRecordIds);
        builder
            .Property<HashSet<TestFeedbackRecordId>>("_feedbackRecordIds")
            .HasColumnName("FeedbackRecordIds")
            .HasEntityIdsHashSetConversion();
    }
}