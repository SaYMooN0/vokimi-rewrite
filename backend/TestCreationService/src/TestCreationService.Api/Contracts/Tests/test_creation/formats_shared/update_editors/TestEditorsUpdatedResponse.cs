using SharedKernel.Common.domain;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared.update_editors;

public class TestEditorsUpdatedResponse
{
    public string[] TestEditorIds { get; init; }
    public TestEditorsUpdatedResponse(IEnumerable<AppUserId> editorIds) {
        TestEditorIds = editorIds.Select(e => e.ToString()).ToArray();
    }
}
