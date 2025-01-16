using SharedKernel.Common.EntityIds;

namespace TestCreationService.Api.Contracts.Tests.test_initialization;

public class NewTestInitializedResponse
{
    public string TestId { get; init; }
    public NewTestInitializedResponse(TestId testId) { TestId = testId.ToString(); }
}
