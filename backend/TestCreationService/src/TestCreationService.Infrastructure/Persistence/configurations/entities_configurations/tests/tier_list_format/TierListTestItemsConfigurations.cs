using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.
    tier_list_format;

public class TierListTestItemsConfigurations : IEntityTypeConfiguration<TierListTestItem>
{
    public void Configure(EntityTypeBuilder<TierListTestItem> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.Content)
            .HasTierListTestItemContentConversion();
    }
}