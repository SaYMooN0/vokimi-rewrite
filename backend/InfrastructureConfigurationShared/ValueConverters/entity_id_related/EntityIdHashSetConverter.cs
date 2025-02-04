using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain;

namespace InfrastructureConfigurationShared.ValueConverters.entity_id_related;

internal class EntityIdHashSetConverter<T> : ValueConverter<HashSet<T>, string> where T : EntityId
{
    public EntityIdHashSetConverter()
        : base(
            ids => string.Join(',', ids.Select(id => id.ToString())),
            str => str.Split(',', StringSplitOptions.RemoveEmptyEntries) 
                .Select(id => (T)Activator.CreateInstance(typeof(T), Guid.Parse(id)!)) 
                .ToHashSet()) 
    { }
}
internal class EntityIdHashSetComparer<T> : ValueComparer<HashSet<T>> where T : EntityId
{
    public EntityIdHashSetComparer() : base(
      (t1, t2) => t1!.SequenceEqual(t2!),
      t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
      t => t) {
    }
}