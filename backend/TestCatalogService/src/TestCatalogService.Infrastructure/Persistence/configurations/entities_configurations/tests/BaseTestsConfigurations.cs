using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCatalogService.Domain.TestAggregate;
using InfrastructureConfigurationShared.Extensions;
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
    }
}
