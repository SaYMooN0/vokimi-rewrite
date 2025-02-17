using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfrastructureConfigurationShared.Extensions;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.AppUserAggregate;

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