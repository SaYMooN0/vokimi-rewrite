using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared;

namespace TestCreationService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot
{
    protected override EntityId EntityId => Id;
    public TestId Id { get; init; }
    public AppUserId CreatorId { get; init; }
    public List<AppUserId> EditorIds { get; init; }
    public TestMainInfo MainInfo { get; init; }
    public TestSettings Settings { get; protected set; }
    public TestStyles Styles { get; init; }
    protected void UpdateTestSettings(TestSettings newTestSettings) => Settings = newTestSettings;
    public ErrOrNothing AddEditor(AppUserId editorId) {
        if (EditorIds.Contains(editorId)) {
            return new Err(message: "This user is already specified as an editor of this test", source: ErrorSource.Client);
        }
        if (EditorIds.Count >= TestRules.MaxTestEditorsCount) {
            return new Err(
                message: "Test editors limit reached",
                details: "You can't add more than " + TestRules.MaxTestEditorsCount + " editors to this test",
                source: ErrorSource.Client
            );
        }
        EditorIds.Add(editorId);
        return ErrOrNothing.Nothing;
    }
}
