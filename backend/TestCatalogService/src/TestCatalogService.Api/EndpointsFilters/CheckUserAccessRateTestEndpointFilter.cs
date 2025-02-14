using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedUserRelationsContext.repository;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Api.EndpointsFilters;

public class CheckUserAccessRateTestEndpointFilter: IEndpointFilter
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IUserFollowingsRepository _userFollowingsRepository;

    public CheckUserAccessRateTestEndpointFilter(
        IBaseTestsRepository baseTestsRepository,
        IUserFollowingsRepository userFollowingsRepository
    ) {
        _baseTestsRepository = baseTestsRepository;
        _userFollowingsRepository = userFollowingsRepository;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        TestId testId = context.HttpContext.GetTestIdFromRoute();
        BaseTest? test = await _baseTestsRepository.GetById(testId);
        if (test is null) {
            return CustomResults.ErrorResponse(Err.ErrPresets.TestNotFound(testId));
        }

        var userId = context.HttpContext.GetAuthenticatedUserId();
        ErrOrNothing access = test.CheckUserAccessToRate(userId, _userFollowingsRepository.GetUserFollowings);

        return access.IsErr(out var err)
            ? CustomResults.ErrorResponse(err)
            : await next(context);
    }
}