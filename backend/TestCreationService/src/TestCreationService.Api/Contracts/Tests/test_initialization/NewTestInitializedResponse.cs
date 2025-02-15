using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestCreationService.Api.Contracts.Tests.test_initialization;

internal class NewTestInitializedResponse
{
    public string TestId { get; init; }
    public NewTestInitializedResponse(TestId testId) { TestId = testId.ToString(); }
}
