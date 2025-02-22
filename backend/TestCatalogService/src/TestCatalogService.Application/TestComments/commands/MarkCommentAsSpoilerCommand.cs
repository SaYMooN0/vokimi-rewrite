using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Application.TestComments.commands;

public record class MarkCommentAsSpoilerCommand(
    TestCommentId CommentId,
    AppUserId UserId
) : IRequest<ErrOrNothing>;

public class MarkCommentAsSpoilerCommandHandler : IRequestHandler<MarkCommentAsSpoilerCommand, ErrOrNothing>
{
    private readonly ITestCommentsRepository _testCommentsRepository;
    private readonly IBaseTestsRepository _baseTestsRepository;

    public MarkCommentAsSpoilerCommandHandler(
        ITestCommentsRepository testCommentsRepository,
        IBaseTestsRepository baseTestsRepository
    ) {
        _testCommentsRepository = testCommentsRepository;
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(
        MarkCommentAsSpoilerCommand request, CancellationToken cancellationToken
    ) {
        TestComment? comment = await _testCommentsRepository.GetById(request.CommentId);
        if (comment is null) {
            return TestCatalogErrPresets.CommentNotFound(request.CommentId);
        }


        var actionRes = await comment.MarkAsSpoiler(request.UserId, _baseTestsRepository);
        if (actionRes.IsErr(out var err)) {
            return err;
        }

        await _testCommentsRepository.Update(comment);
        return ErrOrNothing.Nothing;
    }
}