using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Configs;
using SharedUserRelationsContext.repository;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Api.EndpointsFilters;

public class CheckUserAccessToViewTestEndpointFilter: IEndpointFilter
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IUserFollowingsRepository _userFollowingsRepository;
    private readonly JwtTokenConfig _jwtConfig;

    public CheckUserAccessToViewTestEndpointFilter(
        IBaseTestsRepository baseTestsRepository,
        IUserFollowingsRepository userFollowingsRepository,
        JwtTokenConfig jwtConfig
    ) {
        _baseTestsRepository = baseTestsRepository;
        _userFollowingsRepository = userFollowingsRepository;
        _jwtConfig = jwtConfig;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        TestId testId = context.HttpContext.GetTestIdFromRoute();
        BaseTest? test = await _baseTestsRepository.GetById(testId);
        if (test is null) {
            return CustomResults.ErrorResponse(Err.ErrPresets.TestNotFound(testId));
        }

        var userIdRes = context.HttpContext.ParseUserIdFromJwtToken(_jwtConfig);
        ErrOrNothing access;
        if (userIdRes.IsSuccess(out var userId)) {
            access = await test.CheckUserAccessToViewTest(userId, _userFollowingsRepository.GetUserFollowings);
        }
        else {
            access = test.CheckAccessToViewTestForUnauthorized();
        }

        return access.IsErr(out var err)
            ? CustomResults.ErrorResponse(err)
            : await next(context);
    }
}