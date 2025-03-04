using ApiShared;
using ApiShared.extensions;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.formats_shared.commands.styles;

namespace TestCreationService.Api.Endpoints.test_creation.formats_shared;
internal static class TestCreationStylesHandlers
{
    internal static RouteGroupBuilder MapTestCreationStylesHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/update", UpdateTestStyles)
            .WithRequestValidation<UpdateTestStylesRequest>();
        group.MapPost("/setDefault", SetTestStyledDefault);
        return group;
    }
    private async static Task<IResult> UpdateTestStyles(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateTestStylesRequest>();
        var testId = httpContext.GetTestIdFromRoute();

        UpdateTestStylesCommand command = new(
            testId,
            request.ParsedAccentColor().GetSuccess(),
            request.ParsedErrorsColor().GetSuccess(),
            request.ParsedTestStylesButtons().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
    private async static Task<IResult> SetTestStyledDefault(
       HttpContext httpContext,
       ISender mediator
    ) {
        var testId = httpContext.GetTestIdFromRoute();

        SetTestStylesDefaultCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }

}
