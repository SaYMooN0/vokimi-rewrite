using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.results;

internal record class GeneralFormatTestResultInfoResponse(
    string Id,
    string Name,
    string Text,
    string? Image
)
{
    public static GeneralFormatTestResultInfoResponse FromResult(GeneralTestResult result) => new(
        result.Id.ToString(),
        result.Name,
        result.Text,
        result.Image
    );
}
