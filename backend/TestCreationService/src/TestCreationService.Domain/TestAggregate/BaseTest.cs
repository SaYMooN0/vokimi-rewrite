using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using System.Collections.Immutable;
using TestCreationService.Domain.Common.rules;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

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
    public ErrOrNothing UpdateTestEditors(HashSet<AppUserId> userIds) {
        if (userIds.Count > TestRules.MaxTestEditorsCount) {
            return new Err(
                message: "Too many test editors",
                details: "You can't add more than " + TestRules.MaxTestEditorsCount + " editors to the test"
            );
        }
        if (userIds.Any(id => id == CreatorId)) {
            return new Err(
               message: "Test creator can't be editor",
               details: "Remove the test creator from the list of editors"
           );
        }
        _domainEvents.Add(new TestEditorsListChangedEvent(Id, userIds, EditorIds));
        _editorIds.Clear();
        _editorIds.UnionWith(userIds);
        return ErrOrNothing.Nothing;

    }
    public bool IsUserCreator(AppUserId userId) => userId == CreatorId;
    public HashSet<AppUserId> TestEditorsWithCreator() => new HashSet<AppUserId>(EditorIds) { CreatorId };
}
