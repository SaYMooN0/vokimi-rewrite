using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using TestCatalogService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCatalogService.Infrastructure.Persistence.configurations.extensions;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<T[]> HasEntityIdsArrayConversion<T>(this PropertyBuilder<T[]> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdsArrayConverter<T>(),
            new EntityIdsArrayComparer<T>()
        );
        return builder;
    }
}
