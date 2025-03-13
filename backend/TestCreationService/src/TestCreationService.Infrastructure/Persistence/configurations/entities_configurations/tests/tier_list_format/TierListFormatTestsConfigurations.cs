using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain.entity;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.tier_list_format;
using TestCreationService.Infrastructure.Persistence.configurations.extensions;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.tier_list_format;

internal class TierListFormatTestsConfigurations : IEntityTypeConfiguration<TierListFormatTest>
{
    public void Configure(EntityTypeBuilder<TierListFormatTest> builder) {
        builder.ToTable("TierListFormatTests");
        builder.HasBaseType<BaseTest>();

        builder.OwnsOne<EntitiesOrderController<TierListTestTierId>>("_tiersOrderController",
            controller => {
                controller
                    .Property(p => p.IsShuffled)
                    .HasColumnName("ShuffleTiers");
                controller
                    .Property<Dictionary<TierListTestTierId, ushort>>("_entityOrders")
                    .HasColumnName("TiersOrder")
                    .HasEntityIdsOrderDictionaryConversion();
            }
        );
        builder.OwnsOne<EntitiesOrderController<TierListTestItemId>>("_itemsOrderController",
            controller => {
                controller
                    .Property(p => p.IsShuffled)
                    .HasColumnName("ShuffleItems");
                controller
                    .Property<Dictionary<TierListTestTierId, ushort>>("_entityOrders")
                    .HasColumnName("ItemsOrder")
                    .HasEntityIdsOrderDictionaryConversion();
            }
        );

        builder.HasMany<TierListTestItem>("_items")
            .WithOne()
            .HasForeignKey("TestId");
        
    }
}