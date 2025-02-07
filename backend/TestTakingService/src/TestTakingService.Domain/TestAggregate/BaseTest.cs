using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;

namespace TestTakingService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    public abstract TestFormat Format { get; }
    protected AppUserId _creatorId { get; init; }
    protected ImmutableHashSet<AppUserId> _editors { get; init; }
    protected AccessLevel _acessLevel { get; private set; }

    public async Task<ErrOrNothing> CheckUserAccessToTakeTest(
        AppUserId userId, Func<AppUserId, Task<IEnumerable<AppUserId>>> usersFollowingsGetFunc
    ) {
        if (_acessLevel == AccessLevel.Public) {
            return ErrOrNothing.Nothing;
        }

        if (_acessLevel == AccessLevel.Private) {
            if (_creatorId == userId || _editors.Contains(userId)) {
                return ErrOrNothing.Nothing;
            }

            return Err.ErrFactory.NoAccess("You don't have access to this test. You need to be either creator or editor.");
        }
        //followers only 
    }
}