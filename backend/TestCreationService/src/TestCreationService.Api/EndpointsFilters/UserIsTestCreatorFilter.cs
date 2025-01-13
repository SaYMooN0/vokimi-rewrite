
using ApiShared.extensions;
using ApiShared;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.exceptions;
using TestCreationService.Application.Common.interfaces.repositories;

namespace TestCreationService.Api.EndpointsFilters;

internal class UserIsTestCreatorFilter : IEndpointFilter
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public UserIsTestCreatorFilter(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        var testIdString = context.HttpContext.Request.RouteValues["testId"]?.ToString() ?? "";
        if (!Guid.TryParse(testIdString, out var testGuid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData("Unknown test id"));
        }
        TestId testId = new(testGuid);
        AppUserId userId = context.HttpContext.GetAuthenticatedUserId();
        var isCreatorResult = await _baseTestsRepository.CheckIfUserIsTestCreator(testId, userId);

        if (isCreatorResult.IsErr(out var err)) {
            return CustomResults.ErrorResponse(err);
        }
        bool isCreator = isCreatorResult.IsSuccess();
        if (!isCreator) {
            return CustomResults.ErrorResponse(Err.ErrFactory.NoAccess(
                message: "You must be the test creator to perform this action"
            ));
        }
        return await next(context);
    }
}
