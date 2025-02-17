using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Application.Common.interfaces.repositories;

namespace TestCatalogService.Application.TestComments.commands.list;

public record class ListTestCommentsCommand(
    TestId TestId,
    int PackageNumber,
    AppUserId ViewerId
) : IRequest<ErrOr<ImmutableArray<TestCommentWithViewerVote>>>;

public class ListTestCommentsCommandHandler : IRequestHandler<
    ListTestCommentsCommand,
    ErrOr<ImmutableArray<TestCommentWithViewerVote>>
>
{
    private readonly ITestCommentsRepository _testCommentsRepository;

    public ListTestCommentsCommandHandler(ITestCommentsRepository testCommentsRepository) {
        _testCommentsRepository = testCommentsRepository;
    }

    public async Task<ErrOr<ImmutableArray<TestCommentWithViewerVote>>> Handle(
        ListTestCommentsCommand request,
        CancellationToken cancellationToken
    ) {
        if (request.PackageNumber < 0) {
            return new Err("Package number cannot be negative");
        }

        return await _testCommentsRepository.GetCommentsPackageForViewer(
            request.TestId,
            (uint)request.PackageNumber,
            request.ViewerId
        );
    }
}