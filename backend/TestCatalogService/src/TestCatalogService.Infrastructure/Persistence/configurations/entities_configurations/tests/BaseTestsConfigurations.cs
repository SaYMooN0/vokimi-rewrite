using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCatalogService.Domain.TestAggregate;
using InfrastructureConfigurationShared.Extensions;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.TestAggregate.formats_shared.ratings;
using TestCatalogService.Infrastructure.Persistence.configurations.extensions;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations.tests;

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
            .Property(x => x.EditorIds)
            .HasEntityIdsImmutableArrayConversion();

        builder
            .Property(x => x.CreatorId)
            .HasEntityIdConversion();
        
        builder
            .Property(x => x.Tags)
            .HasTagIdsImmutableHashSetConversion();

        builder.OwnsOne(x => x.InteractionsAccessSettings,
            ias => {
                ias
                    .Property(p => p.TestAccess)
                    .HasColumnName("iaSettings_TestAccess");
                ias
                    .Property(p => p.AllowRatings)
                    .HasResourceAvailabilitySettingConversion()
                    .HasColumnName("iaSettings_AllowRatings");
                ias
                    .Property(p => p.AllowComments)
                    .HasResourceAvailabilitySettingConversion()
                    .HasColumnName("iaSettings_AllowComments");
                ias
                    .Property(p => p.AllowTestTakenPosts)
                    .HasColumnName("iaSettings_AllowTestTakenPosts");
                ias
                    .Property(p => p.AllowTagsSuggestions)
                    .HasResourceAvailabilitySettingConversion()
                    .HasColumnName("iaSettings_AllowTagsSuggestions");
            }
        );
        
        builder
            .Property<HashSet<TestCommentId>>("_commentIds")
            .HasColumnName("CommentIds")
            .HasEntityIdsHashSetConversion();
        
        builder
            .HasMany<TestRating>("_ratings")
            .WithOne()
            .HasForeignKey("TestId");
    }
}