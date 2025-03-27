using InfrastructureConfigurationShared.Extensions;
using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestAggregate;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.tests.tier_list_format_test;

public class TierListFormatTestsConfigurations: IEntityTypeConfiguration<TierListFormatTest>
{
    public void Configure(EntityTypeBuilder<TierListFormatTest> builder) {
        builder.HasBaseType<BaseTest>();
        builder.ToTable("TierListFormatTests");
        
        builder
            .HasMany(x => x.Tiers)
            .WithOne()
            .HasForeignKey("TestId");

        builder
            .HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey("TestId");

        builder
            .Property(x => x.FeedbackOption)
            .HasTierListTestFeedbackOptionConverter()
            .HasColumnName("Feedback");
    }
}