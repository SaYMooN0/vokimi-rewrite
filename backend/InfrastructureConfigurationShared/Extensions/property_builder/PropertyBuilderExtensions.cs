using System.Collections.Immutable;
using InfrastructureConfigurationShared.ValueConverters;
using InfrastructureConfigurationShared.ValueConverters.entity_id_related;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.value_objects;

namespace InfrastructureConfigurationShared.Extensions.property_builder;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TId> HasEntityIdConversion<TId>(this PropertyBuilder<TId> builder)
        where TId : EntityId {
        return builder.HasConversion(new EntityIdConverter<TId>());
    }

    public static PropertyBuilder<TestTagId> HasTestTagIdConversion(this PropertyBuilder<TestTagId> builder) {
        return builder.HasConversion(
            id => id.Value,
            value => new TestTagId(value)
        );
    }

    public static PropertyBuilder<TId?> HasNullableEntityIdConversion<TId>(this PropertyBuilder<TId?> builder)
        where TId : EntityId {
        return builder.HasConversion(new NullableEntityIdConverter<TId>());
    }

    public static PropertyBuilder<HashSet<T>> HasEntityIdsHashSetConversion<T>(
        this PropertyBuilder<HashSet<T>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdHashSetConverter<T>(),
            new EntityIdHashSetComparer<T>()
        );
        return builder;
    }

    public static PropertyBuilder<HexColor> HasHexColorConversion(
        this PropertyBuilder<HexColor> builder
    ) {
        return builder
            .HasConversion(new HexColorConverter())
            .HasMaxLength(7);
    }

    public static PropertyBuilder<TProperty> HasResourceAvailabilitySettingConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new ResourceAvailabilitySettingConverter())
            .HasMaxLength(20);
    }

    public static PropertyBuilder<ImmutableArray<T>> HasEntityIdsImmutableArrayConversion<T>(
        this PropertyBuilder<ImmutableArray<T>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdsImmutableArrayConverter<T>(),
            new EntityIdsImmutableArrayComparer<T>()
        );
        return builder;
    }

    public static PropertyBuilder<ImmutableHashSet<TestTagId>> HasTestTagIdsImmutableHashSetConversion(
        this PropertyBuilder<ImmutableHashSet<TestTagId>> builder
    ) {
        builder.HasConversion(
            new TestTagIdsImmutableHashSetConverter()
        );
        return builder;
    }
}