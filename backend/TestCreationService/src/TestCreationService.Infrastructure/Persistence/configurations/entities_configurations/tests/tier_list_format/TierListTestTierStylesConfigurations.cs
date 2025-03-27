using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.tests.tier_list_format;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.tier_list_format;

public class TierListTestTierStylesConfigurations: IEntityTypeConfiguration<TierListTestTierStyles>
{
    public void Configure(EntityTypeBuilder<TierListTestTierStyles> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder.Property(p => p.TextColor)
            .HasHexColorConversion()
            .HasColumnName("styles_TextColor");
        builder.Property(p => p.BackgroundColor)
            .HasHexColorConversion()
            .HasColumnName("styles_BackgroundColor");
    }
}