using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using System.Collections.Immutable;
using TestCatalogService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCatalogService.Infrastructure.Persistence.configurations.extensions;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableArray<T>> HasEntityIdsImmutableArrayConversion<T>(this PropertyBuilder<ImmutableArray<T>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdsImmutableArrayConverter<T>(),
            new EntityIdsImmutableArrayComparer<T>()
        );
        return builder;
    }
}
