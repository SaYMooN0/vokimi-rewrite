using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;

namespace TestCreationService.Api.EndpointsFilters;

public class TestEditPermissionFilter : IEndpointFilter
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public TestEditPermissionFilter(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        TestId testId = context.HttpContext.GetTestIdFromRoute();
        var usersWithPermissionGetResult = await _baseTestsRepository.GetUserIdsWithPermissionToEditTest(testId);
        if (usersWithPermissionGetResult.IsErr(out var err)) {
            return CustomResults.ErrorResponse(err);
        }

        AppUserId userId = context.HttpContext.GetAuthenticatedUserId();

        if (!usersWithPermissionGetResult.GetSuccess().Contains(userId)) {
            return CustomResults.ErrorResponse(Err.ErrFactory.NoAccess(
                message: "You don't have permission to edit this test",
                details: "You are not the creator and not in the editors list. If you know the creator of this test, you can ask them to add you to the editors list"
            ));
        }

        return await next(context);
    }
}
