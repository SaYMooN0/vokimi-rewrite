
using SharedKernel.Common.errors;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public record TestPublishingProblem(string Area, string Message, string? Details)
{
    public static TestPublishingProblem FromErr(Err err, string area) => new(area, err.Message, err.Details);
}
