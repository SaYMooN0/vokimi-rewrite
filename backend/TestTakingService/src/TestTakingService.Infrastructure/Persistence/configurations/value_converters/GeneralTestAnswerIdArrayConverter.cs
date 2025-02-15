using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestTakingService.Infrastructure.Persistence.configurations.value_converters;

internal class GeneralTestAnswerIdArrayConverter : ValueConverter<ImmutableArray<GeneralTestAnswerId>, Guid[]>
{
    public GeneralTestAnswerIdArrayConverter() : base(
        idArray => idArray.Select(id => id.Value).ToArray(),
        valueArray => valueArray != null
            ? valueArray.Select(value => new GeneralTestAnswerId(value)).ToImmutableArray()
            : ImmutableArray<GeneralTestAnswerId>.Empty
    ) { }
}