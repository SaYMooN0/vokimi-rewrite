using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain;

namespace InfrastructureConfigurationShared.ValueСonverters.entity_id_related;

public class EntityIdConverter<TId> : ValueConverter<TId, Guid> where TId : EntityId
{
    public EntityIdConverter() : base(
        id => id.Value,
        value => (TId)Activator.CreateInstance(typeof(TId), value)) { }
}