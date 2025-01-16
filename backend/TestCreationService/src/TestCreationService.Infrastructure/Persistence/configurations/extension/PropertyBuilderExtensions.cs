using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.EntityIds;
using System.Collections.Immutable;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCreationService.Infrastructure.Persistence.configurations.extension;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> HasResourceAvailabilitySettingConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new ResourceAvailabilitySettingConverter())
            .HasMaxLength(20);
    }
    public static PropertyBuilder<TId> HasEntityIdConversion<TId>(this PropertyBuilder<TId> builder) where TId : EntityId {
        return builder.HasConversion(new EntityIdConverter<TId>());
    }

    public static PropertyBuilder<HashSet<T>> HasEntityIdsHashSetConversion<T>(
        this PropertyBuilder<HashSet<T>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdHashSetConverter<T>(),
            new HashSetOfEntityIdsComparer<T>()
        );
        return builder;
    }
}