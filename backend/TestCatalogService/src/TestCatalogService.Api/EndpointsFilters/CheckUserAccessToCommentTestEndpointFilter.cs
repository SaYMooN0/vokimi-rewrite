using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Configs;
using SharedUserRelationsContext.repository;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Api.EndpointsFilters;

public class CheckUserAccessToCommentTestEndpointFilter : IEndpointFilter
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IUserFollowingsRepository _userFollowingsRepository;
    private readonly JwtTokenConfig _jwtConfig;


    public CheckUserAccessToCommentTestEndpointFilter(
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

        var userId = context.HttpContext.GetAuthenticatedUserId();
        ErrOrNothing access =await test.CheckUserAccessToComment(userId, _userFollowingsRepository.GetUserFollowings);

        return access.IsErr(out var err)
            ? CustomResults.ErrorResponse(err)
            : await next(context);
    }
}