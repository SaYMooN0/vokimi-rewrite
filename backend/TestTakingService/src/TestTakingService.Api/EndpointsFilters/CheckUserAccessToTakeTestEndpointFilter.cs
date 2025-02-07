using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedUserRelationsContext.repository;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Domain.TestAggregate;

namespace TestTakingService.Api.EndpointsFilters;

internal class CheckUserAccessToTakeTestEndpointFilter: IEndpointFilter 
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IUserFollowingsRepository _userFollowingsRepository;

    public CheckUserAccessToTakeTestEndpointFilter(IBaseTestsRepository baseTestsRepository, IUserFollowingsRepository userFollowingsRepository) {
        _baseTestsRepository = baseTestsRepository;
        _userFollowingsRepository = userFollowingsRepository;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        TestId testId = context.HttpContext.GetTestIdFromRoute();
        BaseTest? test = await _baseTestsRepository.GetById(testId);
        if (test is null) {
            return CustomResults.ErrorResponse(Err.ErrPresets.TestNotFound(testId));
        }

        if (test.AccessLevel == AccessLevel.Public) {
            return await next(context);
        }
        AppUserId userId = context.HttpContext.GetAuthenticatedUserId();

        if (!usersWithPermissionGetResult.GetSuccess().Contains(userId)) {
            return CustomResults.ErrorResponse(Err.ErrFactory.NoAccess(
                message: "You don't have permission to edit this test",
                details: "You are not the creator and not in the editors list. If you know the creator of this test, you can ask them to add you to the editors list"
            ));
        }

    }
}