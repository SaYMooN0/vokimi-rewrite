using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;
using TestCatalogService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCatalogService.Infrastructure.Persistence.configurations.extensions;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableHashSet<TestTagId>> HasTagIdsImmutableHashSetConversion(
        this PropertyBuilder<ImmutableHashSet<TestTagId>> builder) {
        builder.HasConversion(
            new TagIdsImmutableHashSetConverter(),
            new TagIdsImmutableHashSetComparer()
        );
        return builder;
    }
}