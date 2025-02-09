using ApiShared.interfaces;

namespace TestTakingService.Api.Contracts.general_format_test.test_taken;

public class GeneralTestTakenRequest(

    ) : IRequestWithValidationNeeded
{
    //chosen answers 
    public RequestValidationResult Validate() => throw new NotImplementedException();
}