using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class EntityIdConverter<TId> : ValueConverter<TId, Guid> where TId : EntityId
{
    public EntityIdConverter() : base(
        id => id.Value,
        value => (TId)Activator.CreateInstance(typeof(TId), value)) { }
}