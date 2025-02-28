using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Api.EndpointsFilters;

public class CheckUserAccessToManageTestEndpointFilter : IEndpointFilter
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    public CheckUserAccessToManageTestEndpointFilter(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        TestId testId = context.HttpContext.GetTestIdFromRoute();
        BaseTest? test = await _baseTestsRepository.GetById(testId);
        if (test is null) {
            return CustomResults.ErrorResponse(Err.ErrPresets.TestNotFound(testId));
        }

        AppUserId userId = context.HttpContext.GetAuthenticatedUserId();
        
        return test.CheckUserAccessToManageTest(userId).IsErr(out var err)
            ? CustomResults.ErrorResponse(err)
            : await next(context);
    }
}