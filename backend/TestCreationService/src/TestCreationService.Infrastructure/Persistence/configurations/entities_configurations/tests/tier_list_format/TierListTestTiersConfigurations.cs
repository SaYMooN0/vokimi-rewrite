using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.tests.tier_list_format;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.
    tier_list_format;

public class TierListTestTiersConfigurations : IEntityTypeConfiguration<TierListTestTier>
{
    public void Configure(EntityTypeBuilder<TierListTestTier> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .HasOne(x => x.Styles)
            .WithOne()
            .HasForeignKey<TierListTestTierStyles>();
    }
}