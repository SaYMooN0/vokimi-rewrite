using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.tier_list_format;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.tests.tier_list_format;


internal class TierListFormatTestsConfigurations : IEntityTypeConfiguration<TierListFormatTest>
{
    public void Configure(EntityTypeBuilder<TierListFormatTest> builder) {
        builder.ToTable("TierListFormatTests");
        builder.HasBaseType<BaseTest>();
        
        builder
            .Property(x => x.FeedbackOption)
            .HasTierListTestFeedbackOptionConverter();
    }
}