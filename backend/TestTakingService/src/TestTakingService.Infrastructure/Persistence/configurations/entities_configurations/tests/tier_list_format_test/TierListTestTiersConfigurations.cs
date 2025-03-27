using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.tests.
    tier_list_format_test;

public class TierListTestTiersConfigurations : IEntityTypeConfiguration<TierListTestTier>
{
    public void Configure(EntityTypeBuilder<TierListTestTier> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder.OwnsOne(x => x.Styles,
            s => {
                s.Property(p => p.TextColor)
                    .HasHexColorConversion()
                    .HasColumnName("styles_TextColor");
                s.Property(p => p.BackgroundColor)
                    .HasHexColorConversion()
                    .HasColumnName("styles_BackgroundColor");
            }
        );
    }
}