using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestTakenRecordAggregate.tier_list_test;
using TestTakingService.Infrastructure.Persistence.configurations.value_converters;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.test_taken_records.
    tier_list_test_taken_records;

public class TierListTestTakenRecordTierDetailsConfigurations
    : IEntityTypeConfiguration<TierListTestTakenRecordTierDetails>
{
    public void Configure(EntityTypeBuilder<TierListTestTakenRecordTierDetails> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.TierId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.ItemsWithOrder)
            .HasConversion(new TierListTestTakenTierDataConverter());
    }
}