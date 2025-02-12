using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.general_format_test.test_taken;

public record class GeneralTestTakenReceivedResultResponse(
    string ReceivedResId
)
{
    //received res 
    public static GeneralTestTakenReceivedResultResponse FromResult(GeneralTestResult result) => new(
        result.Id.ToString()
    );
}