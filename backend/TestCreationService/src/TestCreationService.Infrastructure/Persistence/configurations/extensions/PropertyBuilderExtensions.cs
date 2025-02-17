using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters.general_format_test;

namespace TestCreationService.Infrastructure.Persistence.configurations.extensions;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> HasResourceAvailabilitySettingConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new ResourceAvailabilitySettingConverter())
            .HasMaxLength(20);
    }

    public static PropertyBuilder<Dictionary<T, ushort>> HasEntityIdsOrderDictionaryConversion<T>(
        this PropertyBuilder<Dictionary<T, ushort>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdsOrderDictionaryConverter<T>(),
            new EntityIdsOrderDictionaryComparer<T>()
        );
        return builder;
    }
}