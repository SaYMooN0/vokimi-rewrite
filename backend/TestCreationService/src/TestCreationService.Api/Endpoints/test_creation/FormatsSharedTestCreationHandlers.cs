using ApiShared.extensions;
using ApiShared;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;
using SharedKernel.Common.errors;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Contracts.Tests.test_initialization;
using TestCreationService.Application.Tests.general_format;
using TestCreationService.Application.Tests.formats_shared.commands;

namespace TestCreationService.Api.Endpoints.test_creation;

internal static class FormatsSharedTestCreationHandlers
{
    internal static RouteGroupBuilder MapFormatsSharedTestCreationHandlers(this RouteGroupBuilder group) {
        group.MapPost("/updateTestEditors", UpdateTestEditors)
            .WithRequestValidation<UpdateTestEditorsRequest>()
            .AuthenticationRequired()
            .OnlyByTestCreator();
        return group;
    }
    private async static Task<IResult> UpdateTestEditors(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateTestEditorsRequest>();
        var command = new UpdateTestEditorsCommand(
            TestId: httpContext.GetTestIdFromRoute(),
            EditorIds: request.EditorIds.Select(id => new AppUserId(new Guid(id))).ToArray()
        );
        ErrOr<HashSet<AppUserId>> result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (ids) => Results.Ok(new TestEditorsUpdatedResponse(ids))
        );
    }
}
