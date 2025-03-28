using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain.entity;

namespace InfrastructureConfigurationShared.ValueConverters;

internal class TestTagIdsImmutableHashSetConverter : ValueConverter<ImmutableHashSet<TestTagId>, string>
{
    public TestTagIdsImmutableHashSetConverter()
        : base(
            ids => string.Join(',', ids.Select(id => id.ToString())),
            str => str.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => new TestTagId(id))
                .ToImmutableHashSet()) { }
}