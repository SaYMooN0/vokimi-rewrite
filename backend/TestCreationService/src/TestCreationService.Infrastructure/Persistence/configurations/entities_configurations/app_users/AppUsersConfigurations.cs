using InfrastructureConfigurationShared.Extensions;
using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Infrastructure.Persistence.configurations.extensions;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.app_users;

internal class AppUsersConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder.Ignore(x => x.CreatedTestIds);
        builder
            .Property<HashSet<TestId>>("_createdTestIds")
            .HasColumnName("CreatedTestIds")
            .HasEntityIdsHashSetConversion();

        builder.Ignore(x => x.EditorAssignedTests);
        builder
            .Property<HashSet<TestId>>("_editorAssignedTests")
            .HasColumnName("EditorAssignedTests")
            .HasEntityIdsHashSetConversion();
    }
}
