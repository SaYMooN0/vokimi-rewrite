using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Collections.Immutable;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestResult : Entity<GeneralTestResultId>
{
    public TestId TestId { get; init; }
    public string Name { get; private set; }
    public string? Text { get; private set; }
    public string? Image { get; private set; }
}
