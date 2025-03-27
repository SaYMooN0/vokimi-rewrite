using InfrastructureConfigurationShared.ValueConverters.tier_list_format_test;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.tests.tier_list_format.feedback;
using SharedKernel.Common.tests.tier_list_format.items;

namespace InfrastructureConfigurationShared.Extensions.property_builder;

public static class PropertyBuilderExtensionsForTierListTest
{
    public static PropertyBuilder<TierListTestFeedbackOption> HasTierListTestFeedbackOptionConverter(
        this PropertyBuilder<TierListTestFeedbackOption> builder
    ) => builder.HasConversion(new TierListTestFeedbackOptionConverter());
   
    public static PropertyBuilder<TierListTestItemContentData> HasTierListTestItemContentConversion(
        this PropertyBuilder<TierListTestItemContentData> builder
    ) {
        return builder
            .HasConversion(new TierListTestItemContentDataConverter())
            .HasMaxLength(2040);
    }   
}