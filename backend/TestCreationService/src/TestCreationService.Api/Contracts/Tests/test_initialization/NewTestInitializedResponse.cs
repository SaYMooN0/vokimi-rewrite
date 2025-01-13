using SharedKernel.Common.EntityIds;

namespace TestCreationService.Api.Contracts.Tests.test_initialization;

public class NewTestInitializedResponse
{
    public readonly string TestId;
    public NewTestInitializedResponse(TestId testId) => TestId = testId.ToString();
}
