using ApiShared.extensions;
using ApiShared;
using MediatR;
using SharedKernel.Common.EntityIds;
using TestCreationService.Application.Tests.general_format;
using SharedKernel.Common.errors;
using TestCreationService.Api.Contracts.Tests.test_initialization;
using TestCreationService.Application.Tests.scoring_format;
using OneOf.Types;

namespace TestCreationService.Api.Endpoints;

public static class NewTestInitializationHandlers
{
    internal static RouteGroupBuilder MapNewTestInitializationHandlers(this RouteGroupBuilder group) {
        group.MapPost("/general", CreateNewGeneralFormatTest)
            .AuthenticationRequired()
            .WithRequestValidation<InitNewTestRequest>();

        group.MapPost("/scoring", CreateNewScoringFormatTest)
            .AuthenticationRequired()
            .WithRequestValidation<InitNewTestRequest>();
        return group;
    }
    private async static Task<IResult> CreateNewGeneralFormatTest(
        HttpContext httpContext,
        ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<InitNewTestRequest>();
        var creator = httpContext.GetAuthenticatedUserId();
        var editors = request.EditorIds
            .Select(id => new AppUserId(new(id)))
            .Where(id => id != creator)
            .ToHashSet();

        InitGeneralFormatTestCommand command = new(request.TestName, creator, editors);
        ErrOr<TestId> result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (id) => Results.Json(new NewTestInitializedResponse(id), statusCode: 201)
        );
    }
    private async static Task<IResult> CreateNewScoringFormatTest(
        HttpContext httpContext,
        ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<InitNewTestRequest>();
        var creator = httpContext.GetAuthenticatedUserId();
        var editors = request.EditorIds
            .Select(id => new AppUserId(new(id)))
            .Where(id => id != creator)
            .ToHashSet();

        InitScoringFormatTestCommand command = new(request.TestName, creator, editors);
        ErrOr<TestId> result = await mediator.Send(command);
        return CustomResults.FromErrOr(
            result,
            (id) => Results.Json(new NewTestInitializedResponse(id), statusCode: 201)
        );
    }
}
