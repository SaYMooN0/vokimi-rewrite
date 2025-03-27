using InfrastructureConfigurationShared.Extensions;
using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations;

internal class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property<HashSet<TestId>>("_createdTestIds")
            .HasColumnName("CreatedTestIds")
            .HasEntityIdsHashSetConversion();

        builder
            .Property<HashSet<TestId>>("_editorAssignedTests")
            .HasColumnName("EditorAssignedTests")
            .HasEntityIdsHashSetConversion();

        builder
            .Property<HashSet<TestRatingId>>("_ratingIds")
            .HasColumnName("RatingIds")
            .HasEntityIdsHashSetConversion();

        builder
            .Property<HashSet<TestCommentId>>("_commentIds")
            .HasColumnName("CommentIds")
            .HasEntityIdsHashSetConversion();
    }
}