using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.tests;
using System.Collections.Immutable;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    public abstract TestFormat Format { get; }
    public string Name { get; init; }
    public string CoverImg { get; init; }
    public string Description { get; init; }
    public AppUserId CreatorId { get; init; }
    public ImmutableArray<AppUserId> EditorIds { get; init; }
    public DateTime PublicationDate { get; init; }
    public Language Language { get; init; }
    public ImmutableHashSet<TestTagId> Tags { get; protected set; }

    //comments 
    //ratings
    public ErrOrNothing CheckAccessToViewTestForUnauthorized() {
    }

    public async Task<ErrOrNothing> CheckUserAccessToViewTest(AppUserId userId, Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings) {
    }

    public async Task<ErrOrNothing> CheckUserAccessToViewTestComments(AppUserId userId, Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings) {
    }

    public ErrOrNothing CheckAccessToViewTestCommentsForUnauthorized() {
    }

    public async Task<ErrOrNothing> CheckUserAccessToCommentTest(AppUserId userId, Func<AppUserId, Task<ImmutableArray<AppUserId>>> getUserFollowings) {
    }

    public ErrOrNothing CheckAccessToCommentTestForUnauthorized() {
        throw new ();
    }

    public ErrOrNothing CheckUserAccessToRate() {
    }
}