using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Infrastructure.Persistence.configurations.value_converters;

internal class TagIdsImmutableHashSetConverter : ValueConverter<ImmutableHashSet<TestTagId>, string[]>
{
    public TagIdsImmutableHashSetConverter()
        : base(
            ids => ids.Select(id => id.ToString()).ToArray(),
            ids => TestTagIdsFromStringArr(ids).ToImmutableHashSet()
        ) { }

    private static IEnumerable<TestTagId> TestTagIdsFromStringArr(string[] ids) =>
        ids.Select(id => {
            var res = TestTagId.Create(id);
            if (res.IsErr(out var err)) {
                throw new ErrCausedException(new Err($"Invalid tag value in the database id: {id}"));
            }
            return res.GetSuccess();
        });
}

internal class TagIdsImmutableHashSetComparer : ValueComparer<ImmutableHashSet<TestTagId>>
{
    public TagIdsImmutableHashSetComparer() : base(
        (t1, t2) => false,
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t) { }
}