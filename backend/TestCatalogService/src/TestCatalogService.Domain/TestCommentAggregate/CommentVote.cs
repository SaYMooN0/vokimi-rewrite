using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestCommentAggregate;

public class CommentVote : Entity<CommentVoteId>
{
    private CommentVote() { }
    public AppUserId UserId { get; init; }
    public bool IsUp { get; private set; }

    public CommentVote(AppUserId userId, bool isUp) {
        Id = CommentVoteId.CreateNew();
        UserId = userId;
        IsUp = isUp;
    }

    public ErrOrNothing ChangeIsUpValue(bool newIsUp) {
        if (IsUp == newIsUp) {
            return new Err("Comment had the same vote before");
        }

        IsUp = newIsUp;
        return ErrOrNothing.Nothing;
    }
}