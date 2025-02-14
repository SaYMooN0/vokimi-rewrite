using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.value_objects;
using System.Collections.Immutable;
using SharedKernel.Common.tests.formats_shared.interaction_access_settings;
using SharedKernel.Common.tests.formats_shared.test_styles;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    protected AppUserId CreatorId { get; private set; }
    protected readonly HashSet<AppUserId> _editorIds = new();
    public ImmutableHashSet<AppUserId> EditorIds => _editorIds.ToImmutableHashSet();
    public abstract TestFormat Format { get; }
    protected TestMainInfo _mainInfo { get; init; }
    protected TestInteractionsAccessSettings _interactionsAccessSettings { get; init; }
    protected TestStylesSheet _styles { get; init; }
    protected TestTagsList _tags { get; init; }

    protected BaseTest(
        TestId id,
        AppUserId creatorId,    
        HashSet<AppUserId> editorIds,
        TestMainInfo mainInfo
    ) {
        Id = id;
        CreatorId = creatorId;
        _editorIds = editorIds;
        _mainInfo = mainInfo;
        _interactionsAccessSettings = TestInteractionsAccessSettings.CreateNew();
        _styles = TestStylesSheet.CreateNew(id);
        _tags = TestTagsList.CreateNew(id);
    }

    public ErrOrNothing UpdateEditors(HashSet<AppUserId> newEditors) {
        if (newEditors.Count > TestRules.MaxTestEditorsCount) {
            return new Err(
                message: "Too many test editors",
                details: "You can't add more than " + TestRules.MaxTestEditorsCount + " editors to the test"
            );
        }

        if (newEditors.Any(id => id == CreatorId)) {
            return new Err(
                message: "Test creator can't be editor",
                details: "Remove the test creator from the list of editors"
            );
        }

        _domainEvents.Add(new TestEditorsListChangedEvent(Id, newEditors, EditorIds));
        _editorIds.Clear();
        _editorIds.UnionWith(newEditors);
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing ChangeTestCreator(AppUserId newCreatorId, bool keepCurrentAsEditor) {
        if (CreatorId == newCreatorId) {
            return new Err(message: "Specified user is already set as test creator");
        }

        if (!_editorIds.Contains(newCreatorId)) {
            return new Err(
                message:
                "User you want to make creator is not currently an editor. Only editors can become test creator. If you still want to make this user creator, add them as editor first"
            );
        }

        var oldEditors = _editorIds.ToImmutableHashSet();
        _editorIds.Remove(newCreatorId);
        if (keepCurrentAsEditor) {
            _editorIds.Add(CreatorId);
        }

        var oldCreator = CreatorId;
        CreatorId = newCreatorId;
        _domainEvents.Add(new TestCreatorChangedEvent(Id, OldCreator: oldCreator, NewCreator: newCreatorId));
        _domainEvents.Add(new TestEditorsListChangedEvent(Id, EditorIds, oldEditors));
        return ErrOrNothing.Nothing;
    }

    public bool IsUserCreator(AppUserId userId) => userId == CreatorId;
    public HashSet<AppUserId> TestEditorsWithCreator() => new HashSet<AppUserId>(EditorIds) { CreatorId };

    public ErrOrNothing UpdateMainInfo(string testName, string description, Language language) =>
        _mainInfo.Update(testName, description, language);

    public ErrOrNothing UpdateCoverImg(string coverImg) {
        return _mainInfo.UpdateCoverImg(coverImg);
        //interaction event for img service...
    }

    public ErrListOrNothing UpdateInteractionsAccessSettings(
        AccessLevel testAccessLevel,
        ResourceAvailabilitySetting ratingsSetting,
        ResourceAvailabilitySetting commentsSetting,
        bool allowTestTakenPosts,
        ResourceAvailabilitySetting tagsSuggestionsSetting
    ) => _interactionsAccessSettings.Update(
        testAccessLevel,
        ratingsSetting: ratingsSetting,
        commentsSetting: commentsSetting,
        allowTestTakenPosts,
        tagsSuggestionsSetting
    );

    public void UpdateStyles(HexColor accentColor, HexColor errorsColor, TestStylesButtons buttonsStyle) =>
        _styles.Update(accentColor, errorsColor, buttonsStyle);

    public void SetStylesDefault() =>
        _styles.SetToDefault();

    public ISet<string> GetTags() => _tags.GetTags();
    public ErrListOr<ISet<string>> UpdateTags(IEnumerable<string> newTags) => _tags.Update(newTags);
    public void ClearTags() => _tags.Clear();
    public abstract void DeleteTest();
}