using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common.filters;
using TestCatalogService.Domain.Common.interfaces.repositories;

namespace TestCatalogService.Application.TestComments.commands.list_comments;

public record class ListFilteredTestCommentsCommand(
    TestId TestId,
    ListTestCommentsFilter Filter,
    int PackageNumber,
    AppUserId? ViewerId
) : IRequest<ErrOr<ImmutableArray<TestCommentWithViewerVote>>>;

public class ListFilteredTestCommentsCommandHandler : IRequestHandler<
    ListFilteredTestCommentsCommand,
    ErrOr<ImmutableArray<TestCommentWithViewerVote>>
>
{
    private readonly ITestCommentsRepository _testCommentsRepository;

    public ListFilteredTestCommentsCommandHandler(ITestCommentsRepository testCommentsRepository) {
        _testCommentsRepository = testCommentsRepository;
    }

    public async Task<ErrOr<ImmutableArray<TestCommentWithViewerVote>>> Handle(
        ListFilteredTestCommentsCommand request,
        CancellationToken cancellationToken
    ) {
        if (request.PackageNumber < 0) {
            return new Err("Package number cannot be negative");
        }

        return await _testCommentsRepository.GetFilteredCommentsPackageForViewer(
            request.TestId,
            (uint)request.PackageNumber,
            request.ViewerId,
            request.Filter
        );
    }
}