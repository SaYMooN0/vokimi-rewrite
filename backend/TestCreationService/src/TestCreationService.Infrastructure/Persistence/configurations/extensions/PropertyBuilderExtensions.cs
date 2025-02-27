using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCreationService.Infrastructure.Persistence.configurations.extensions;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<Dictionary<T, ushort>> HasEntityIdsOrderDictionaryConversion<T>(
        this PropertyBuilder<Dictionary<T, ushort>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdsOrderDictionaryConverter<T>(),
            new EntityIdsOrderDictionaryComparer<T>()
        );
        return builder;
    }
}