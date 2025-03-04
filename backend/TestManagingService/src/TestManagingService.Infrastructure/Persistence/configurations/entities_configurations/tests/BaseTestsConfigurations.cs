using System.Collections.Immutable;
using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManagingService.Domain.Common;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.formats_shared.comment_reports;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.tests;

internal class BaseTestsConfigurations : IEntityTypeConfiguration<BaseTest>
{
    public void Configure(EntityTypeBuilder<BaseTest> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.CreatorId)
            .HasEntityIdConversion();
        builder
            .Property(x => x.EditorIds)
            .HasEntityIdsImmutableArrayConversion();

        builder
            .Property<ImmutableArray<TestFeedbackRecordId>>("_feedbackRecords")
            .HasEntityIdsImmutableArrayConversion();

        builder
            .HasMany<TestCommentReport>("_commentReports")
            .WithOne()
            .HasForeignKey("TestId");
    }
}