using InfrastructureConfigurationShared.ValueСonverters.entity_id_related;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;

namespace InfrastructureConfigurationShared.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TId> HasEntityIdConversion<TId>(this PropertyBuilder<TId> builder) where TId : EntityId {
        return builder.HasConversion(new EntityIdConverter<TId>());
    }
}