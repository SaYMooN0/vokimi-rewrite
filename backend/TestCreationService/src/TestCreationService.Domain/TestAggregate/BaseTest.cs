using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared;

namespace TestCreationService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot
{
    protected BaseTest() { }

    protected override EntityId EntityId => Id;
    public TestId Id { get; init; }
    protected AppUserId CreatorId { get; init; }
    private readonly HashSet<AppUserId> _editorIds = new();
    public ImmutableHashSet<AppUserId> EditorIds => _editorIds.ToImmutableHashSet();
    public abstract TestFormat Format { get; }
    public TestMainInfo MainInfo { get; init; }
    public TestSettings Settings { get; protected set; }
    public TestStyles Styles { get; init; }

    protected BaseTest(
        TestId id,
        AppUserId creatorId,
        HashSet<AppUserId> editorIds,
        TestMainInfo mainInfo,
        TestSettings settings,
        TestStyles styles
    ) {
        Id = id;
        CreatorId = creatorId;
        _editorIds = editorIds;
        MainInfo = mainInfo;
        Settings = settings;
        Styles = styles;
    }
    protected void UpdateTestSettings(TestSettings newTestSettings) => Settings = newTestSettings;
    public ErrOrNothing AddEditor(AppUserId editorId) {
        if (EditorIds.Contains(editorId)) {
            return new Err(
                message: "User is already specified as an editor of this test",
                details: $"User {editorId} is already an editor",
                source: ErrorSource.Client
            );
        }
        if (EditorIds.Count >= TestRules.MaxTestEditorsCount) {
            return new Err(
                message: "Test editors limit reached",
                details: "You can't add more than " + TestRules.MaxTestEditorsCount + " editors to this test"
            );
        }
        EditorIds.Add(editorId);
        return ErrOrNothing.Nothing;
    }
    public bool IsUserCreator(AppUserId userId) => userId == CreatorId;
    public HashSet<AppUserId> TestEditorsWithCreator() => new HashSet<AppUserId>(EditorIds) { CreatorId };
}
