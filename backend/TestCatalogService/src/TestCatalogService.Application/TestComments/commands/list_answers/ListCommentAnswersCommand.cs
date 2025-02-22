using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.interfaces.repositories;

namespace TestCatalogService.Application.TestComments.commands.list_answers;

public record ListCommentAnswersCommand(
    TestCommentId ParentCommentIdId,
    int PackageNumber,
    AppUserId? ViewerId
) : IRequest<ErrOr<ImmutableArray<TestCommentWithViewerVote>>>;

public class ListCommentAnswersCommandHandler
    : IRequestHandler<ListCommentAnswersCommand, ErrOr<ImmutableArray<TestCommentWithViewerVote>>>
{
    private readonly ITestCommentsRepository _testCommentsRepository;

    public ListCommentAnswersCommandHandler(ITestCommentsRepository testCommentsRepository) {
        _testCommentsRepository = testCommentsRepository;
    }

    public async Task<ErrOr<ImmutableArray<TestCommentWithViewerVote>>> Handle(
        ListCommentAnswersCommand request,
        CancellationToken cancellationToken
    ) {
        if (request.PackageNumber < 0) {
            return new Err("Package number cannot be negative");
        }

        return await _testCommentsRepository.GetAnswersPackageForViewer(
            request.ParentCommentIdId,
            (uint)request.PackageNumber,
            request.ViewerId
        );
    }
}