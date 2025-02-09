using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.test_styles;

namespace TestTakingService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    public delegate Task<ImmutableArray<AppUserId>> GetUserFollowingsAsyncDelegate(AppUserId userId);

    private const string NoAccessPrivateMessage =
        "You don't have access to this test. You need to be either the creator or an editor of this test";

    private const string NoAccessFollowersMessage =
        "You need to follow the test creator to access this test";

    protected BaseTest() { }

    protected BaseTest(
        TestId id,
        AppUserId creatorId,
        ImmutableHashSet<AppUserId> editors,
        AccessLevel accessLevel,
        TestStylesSheet styles
    ) {
        Id = id;
        _creatorId = creatorId;
        _editors = editors;
        _accessLevel = accessLevel;
        Styles = styles;
    }

    public abstract TestFormat Format { get; }
    protected AppUserId _creatorId { get; init; }
    protected ImmutableHashSet<AppUserId> _editors { get; init; }
    protected AccessLevel _accessLevel { get; init; }
    public TestStylesSheet Styles { get; init; }

    public ErrOrNothing CheckAccessToTakeTestForUnauthorized() =>
        _accessLevel switch {
            AccessLevel.Public => ErrOrNothing.Nothing,
            AccessLevel.Private => Err.ErrFactory.NoAccess(NoAccessPrivateMessage),
            AccessLevel.FollowersOnly => Err.ErrFactory.NoAccess(NoAccessFollowersMessage),
            _ => Err.ErrFactory.NoAccess("This test has unknown access level")
        };

    public async Task<ErrOrNothing> CheckUserAccessToTakeTest(
        AppUserId userId,
        GetUserFollowingsAsyncDelegate getUserFollowingsAsync
    ) => _accessLevel switch {
        AccessLevel.Public => ErrOrNothing.Nothing,
        AccessLevel.Private =>
            _creatorId == userId || _editors.Contains(userId)
                ? ErrOrNothing.Nothing
                : Err.ErrFactory.NoAccess(NoAccessPrivateMessage),
        AccessLevel.FollowersOnly => await CheckFollowersAccess(userId, getUserFollowingsAsync),
        _ => Err.ErrFactory.NoAccess("This test has unknown access level")
    };

    private async Task<ErrOrNothing> CheckFollowersAccess(
        AppUserId userId,
        GetUserFollowingsAsyncDelegate getUserFollowingsAsync
    ) {
        IEnumerable<AppUserId> followings = await getUserFollowingsAsync(userId);

        return followings.Contains(_creatorId)
            ? ErrOrNothing.Nothing
            : Err.ErrFactory.NoAccess(
                NoAccessFollowersMessage,
                $"Test creator Id: {_creatorId}"
            );
    }
}