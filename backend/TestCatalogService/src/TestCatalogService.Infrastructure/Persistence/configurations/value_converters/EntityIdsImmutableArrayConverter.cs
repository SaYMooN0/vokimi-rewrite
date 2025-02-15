using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain;
using System.Collections.Immutable;
using SharedKernel.Common.domain.entity_id;

namespace TestCatalogService.Infrastructure.Persistence.configurations.value_converters;
internal class EntityIdsImmutableArrayConverter<T> : ValueConverter<ImmutableArray<T>, string> where T : EntityId
{
    public EntityIdsImmutableArrayConverter()
        : base(
            ids => string.Join(',', ids.Select(id => id.ToString())),
            str => str.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => (T)Activator.CreateInstance(typeof(T), Guid.Parse(id)!))
                .ToImmutableArray<T>()) { }
}
internal class EntityIdsImmutableArrayComparer<T> : ValueComparer<ImmutableArray<T>> where T : EntityId
{
    public EntityIdsImmutableArrayComparer() : base(
      (t1, t2) => false,
      t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
      t => t) {
    }
}