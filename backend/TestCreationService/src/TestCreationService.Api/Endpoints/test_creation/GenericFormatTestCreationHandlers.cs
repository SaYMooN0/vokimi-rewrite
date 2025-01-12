using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Api.Contracts.Tests.formats_shared;
using TestCreationService.Api.Contracts.Tests.generic_format.requests;
using TestCreationService.Application.Tests.generic_format;

namespace TestCreationService.Api.Endpoints.test_creation;

internal static class GenericFormatTestCreationHandlers
{
    internal static RouteGroupBuilder MapGenericFormatTestCreationHandlers(this RouteGroupBuilder group) {
        group.MapPost("/new", CreateNewGenericFormatTest)
            .AuthenticationRequired()
            .WithRequestValidation<CreateNewGenericFormatTestRequest>();
        return group;
    }
    private async static Task<IResult> CreateNewGenericFormatTest(
        HttpContext httpContext,
        ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<CreateNewGenericFormatTestRequest>();
        var command = new CreateNewGenericFormatTestCommand(
            testName: request.Name,
            creatorId: httpContext.GetAuthenticatedUserId(),
            editorIds: request.EditorIds.Select(id => new AppUserId(new Guid(id))).ToArray()
        );
        ErrOr<TestId> result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (id) => Results.Ok(new NewTestCreatedResponse(id))
        );
    }
}
