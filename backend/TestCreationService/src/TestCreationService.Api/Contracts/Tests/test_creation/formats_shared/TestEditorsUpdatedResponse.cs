using SharedKernel.Common.EntityIds;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

public class TestEditorsUpdatedResponse
{
    private string[] TestEditorIds { get; init; }
    public TestEditorsUpdatedResponse(IEnumerable<AppUserId> editorIds) {
        TestEditorIds = editorIds.Select(e => e.ToString()).ToArray();
    }
}
