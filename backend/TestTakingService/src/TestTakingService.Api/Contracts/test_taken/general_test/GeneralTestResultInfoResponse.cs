using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.test_taken.general_test;

public record class GeneralTestResultInfoResponse(
    string Id,
    string Name,
    string Text,
    string Image
)
{
    public static GeneralTestResultInfoResponse FromResult(GeneralTestResult result) => new(
        result.Id.ToString(),
        result.Name,
        result.Text,
        result.Image
    );
}