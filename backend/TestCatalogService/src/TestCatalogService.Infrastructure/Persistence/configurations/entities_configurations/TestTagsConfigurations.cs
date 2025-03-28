using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCatalogService.Domain.TestTagAggregate;
using  InfrastructureConfigurationShared.Extensions.property_builder;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations;

internal class TestTagsConfigurations : IEntityTypeConfiguration<TestTag>
{
    public void Configure(EntityTypeBuilder<TestTag> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasTestTagIdConversion();
    }
}