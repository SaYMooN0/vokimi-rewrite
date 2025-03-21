using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Api.Contracts.test_taken.general_test;

public record class GeneralTestTakenReceivedResultResponse(
    string ReceivedResId
)
{
    //received res 
    public static GeneralTestTakenReceivedResultResponse FromResult(GeneralTestResult result) => new(
        result.Id.ToString()
    );
}