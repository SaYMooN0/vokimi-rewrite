using SharedKernel.Common.EntityIds;

namespace TestCreationService.Api.Contracts.Tests.formats_shared;

public class NewTestCreatedResponse
{
    public readonly string TestId;
    public NewTestCreatedResponse(TestId testId) => TestId = testId.ToString();
}
