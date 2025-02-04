using InfrastructureConfigurationShared.ValueConverters.entity_id_related;
using InfrastructureConfigurationShared.ValueСonverters.entity_id_related;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;

namespace InfrastructureConfigurationShared.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TId> HasEntityIdConversion<TId>(this PropertyBuilder<TId> builder)
        where TId : EntityId {
        return builder.HasConversion(new EntityIdConverter<TId>());
    }

    public static PropertyBuilder<HashSet<T>> HasEntityIdsHashSetConversion<T>(
        this PropertyBuilder<HashSet<T>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdHashSetConverter<T>(),
            new EntityIdHashSetComparer<T>()
        );
        return builder;
    }
}