using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace InfrastructureConfigurationShared.ValueConverters.entity_id_related;

public class NullableEntityIdConverter<TId> : ValueConverter<TId?, Guid?> where TId : EntityId
{
    public NullableEntityIdConverter() : base(
        id => NullableEntityIdToGuid(id),
        value => value.HasValue
            ? (TId)Activator.CreateInstance(typeof(TId), value.Value)
            : null
    ) { }

    private static Guid? NullableEntityIdToGuid(EntityId? id) => id?.Value;
}