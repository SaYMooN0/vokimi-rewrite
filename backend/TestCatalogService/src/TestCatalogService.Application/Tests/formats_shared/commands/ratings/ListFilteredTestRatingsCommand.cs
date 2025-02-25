using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedUserRelationsContext.repository;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.Common.filters;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.formats_shared.ratings;

namespace TestCatalogService.Application.Tests.formats_shared.commands.ratings;

public record class ListFilteredTestRatingsCommand(
    TestId TestId,
    AppUserId? ViewerId,
    ListTestRatingsFilter Filter,
    int Package
) : IRequest<ErrOr<ImmutableArray<TestRating>>> { }

public class ListFilteredTestRatingsCommandHandler
    : IRequestHandler<ListFilteredTestRatingsCommand, ErrOr<ImmutableArray<TestRating>>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IUserFollowingsRepository _userFollowingsRepository;

    public ListFilteredTestRatingsCommandHandler(
        IBaseTestsRepository baseTestsRepository,
        IUserFollowingsRepository userFollowingsRepository
    ) {
        _baseTestsRepository = baseTestsRepository;
        _userFollowingsRepository = userFollowingsRepository;
    }

    public async Task<ErrOr<ImmutableArray<TestRating>>> Handle(
        ListFilteredTestRatingsCommand request,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetWithRatingsAsNoTracking(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return await test.GetFilteredRatingsPackage(
            request.ViewerId,
            _userFollowingsRepository,
            request.Filter,
            request.Package
        );
    }
}