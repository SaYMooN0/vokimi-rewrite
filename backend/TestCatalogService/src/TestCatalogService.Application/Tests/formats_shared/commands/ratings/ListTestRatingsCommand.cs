using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared;

namespace TestCatalogService.Application.Tests.formats_shared.commands.ratings;

public record class ListTestRatingsCommand(
    TestId TestId,
    int Package
) : IRequest<ErrOr<ImmutableArray<TestRating>>>;

public class ListTestRatingsCommandHandler
    : IRequestHandler<ListTestRatingsCommand, ErrOr<ImmutableArray<TestRating>>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public ListTestRatingsCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOr<ImmutableArray<TestRating>>> Handle(
        ListTestRatingsCommand request,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetWithRatingsAsNoTracking(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return test.GetRatingsPackage(request.Package);
    }
}