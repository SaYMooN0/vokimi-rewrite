using ApiShared.extensions;
using ApiShared;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestCreationService.Api.EndpointsFilters;

internal class UserIsTestCreatorFilter : IEndpointFilter
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public UserIsTestCreatorFilter(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        TestId testId = context.HttpContext.GetTestIdFromRoute();
        AppUserId userId = context.HttpContext.GetAuthenticatedUserId();
        var isCreatorResult = await _baseTestsRepository.CheckIfUserIsTestCreator(testId, userId);

        if (isCreatorResult.IsErr(out var err)) {
            return CustomResults.ErrorResponse(err);
        }
        bool isCreator = isCreatorResult.GetSuccess();
        if (!isCreator) {
            return CustomResults.ErrorResponse(Err.ErrFactory.NoAccess(
                message: "You must be the test creator to perform this action"
            ));
        }
        return await next(context);
    }
}
