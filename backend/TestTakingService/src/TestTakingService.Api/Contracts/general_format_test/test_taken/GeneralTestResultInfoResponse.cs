using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.general_format_test.test_taken;

public record class GeneralTestResultInfoResponse(
    string Id
)
{
    public static GeneralTestResultInfoResponse FromResult(GeneralTestResult result) => new(
        result.Id.ToString()
    );
}