using ApiShared.extensions;
using ApiShared;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Endpoints.test_creation;

internal static class FormatsSharedTestCreationHandlers
{
    internal static RouteGroupBuilder MapFormatsSharedTestCreationHandlers(this RouteGroupBuilder group) {
        group.MapPost("/addNewTestEditors", AddTestEditorsToTest)
            .WithRequestValidation<AddEditorsToTestRequest>()
            .AuthenticationRequired()
            .OnlyByTestCreator();
        return group;
    }
    private async static Task<IResult> AddTestEditorsToTest(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<AddEditorsToTestRequest>();
        return CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented());
    }
}
