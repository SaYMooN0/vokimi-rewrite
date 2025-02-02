using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain;

namespace TestCatalogService.Infrastructure.Persistence.configurations.value_converters;
internal class EntityIdsArrayConverter<T> : ValueConverter<T[], string> where T : EntityId
{
    public EntityIdsArrayConverter()
        : base(
            ids => string.Join(',', ids.Select(id => id.ToString())),
            str => str.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => (T)Activator.CreateInstance(typeof(T), Guid.Parse(id)!))
                .ToArray()) { }
}
internal class EntityIdsArrayComparer<T> : ValueComparer<T[]> where T : EntityId
{
    public EntityIdsArrayComparer() : base(
      (t1, t2) => false,
      t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
      t => t) {
    }
}