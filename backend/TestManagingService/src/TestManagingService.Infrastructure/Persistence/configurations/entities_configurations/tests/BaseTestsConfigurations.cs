using System.Collections.Immutable;
using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.Common;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.formats_shared;
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
            .HasColumnName("FeedbackRecords")
            .HasEntityIdsImmutableArrayConversion();

        builder
            .HasMany<TestCommentReport>("_commentReports")
            .WithOne()
            .HasForeignKey("TestId");

        builder.Ignore(x => x.Tags);
        builder
            .Property<ImmutableHashSet<TestTagId>>("_tags")
            .HasColumnName("Tags")
            .HasTestTagIdsImmutableHashSetConversion();

        builder.Ignore(x => x.TagSuggestions);
        builder
            .HasMany<TagSuggestionForTest>("_tagSuggestions")
            .WithOne()
            .HasForeignKey("TestId");

        builder
            .Property<ImmutableHashSet<TestTagId>>("_tagsBannedFromSuggestion")
            .HasColumnName("TagsBannedFromSuggestion")
            .HasTestTagIdsImmutableHashSetConversion();
    }
}