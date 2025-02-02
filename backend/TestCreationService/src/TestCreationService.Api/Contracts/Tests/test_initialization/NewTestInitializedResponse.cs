using SharedKernel.Common.domain;

namespace TestCreationService.Api.Contracts.Tests.test_initialization;

internal class NewTestInitializedResponse
{
    public string TestId { get; init; }
    public NewTestInitializedResponse(TestId testId) { TestId = testId.ToString(); }
}
