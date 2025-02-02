using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class EntityIdsOrderDictionaryConverter<T> : ValueConverter<Dictionary<T, ushort>, string> where T : EntityId
{
    public EntityIdsOrderDictionaryConverter()
        : base(
            dictionary => string.Join(',', dictionary.Select(kvp => kvp.Key.ToString() + ':' + kvp.Value)),
            str => str
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToDictionary(
                    kvp => (T)Activator.CreateInstance(typeof(T), Guid.Parse(kvp.Split(':', StringSplitOptions.RemoveEmptyEntries)[0])),
                    kvp => ushort.Parse(kvp.Split(':', StringSplitOptions.RemoveEmptyEntries)[1])
                )
        ) { }

}
internal class EntityIdsOrderDictionaryComparer<T> : ValueComparer<Dictionary<T, ushort>> where T : EntityId
{
    public EntityIdsOrderDictionaryComparer() : base(
      (t1, t2) => false,
      t => t.Select(x => (x!.Key.GetHashCode() ^ x.Value.GetHashCode())).Aggregate((x, y) => x ^ y),
      t => t
    ) {
    }
}