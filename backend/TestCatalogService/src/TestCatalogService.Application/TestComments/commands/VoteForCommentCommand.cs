using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Application.TestComments.commands;

public record class VoteForCommentCommand(
    TestCommentId CommentId,
    AppUserId UserId,
    bool IsUp
) : IRequest<ErrOr<UserCommentVoteState>>;

public class VoteForCommentCommandHandler : IRequestHandler<VoteForCommentCommand, ErrOr<UserCommentVoteState>>
{
    private readonly ITestCommentsRepository _testCommentsRepository;

    public VoteForCommentCommandHandler(ITestCommentsRepository testCommentsRepository) {
        _testCommentsRepository = testCommentsRepository;
    }

    public async Task<ErrOr<UserCommentVoteState>> Handle(
        VoteForCommentCommand request,
        CancellationToken cancellationToken
    ) {
        TestComment? comment = await _testCommentsRepository.GetById(request.CommentId);
        if (comment is null) {
            return TestCatalogErrPresets.CommentNotFound(request.CommentId);
        }

        var voteRes = comment.Vote(request.UserId, request.IsUp);
        if (voteRes.IsErr(out var err)) {
            return err;
        }

        await _testCommentsRepository.Update(comment);
        return voteRes.GetSuccess();
    }
}