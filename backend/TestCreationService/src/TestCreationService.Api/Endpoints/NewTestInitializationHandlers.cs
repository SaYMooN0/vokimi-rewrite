using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Api.Contracts.Tests.test_initialization;
using TestCreationService.Application.Tests.general_format;
using TestCreationService.Application.Tests.general_format.commands;
using TestCreationService.Application.Tests.scoring_format.commands;

namespace TestCreationService.Api.Endpoints;

public static class NewTestInitializationHandlers
{
    internal static RouteGroupBuilder MapNewTestInitializationHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired();
        
        group.MapPost("/general", CreateNewGeneralFormatTest)
            .WithRequestValidation<InitNewTestRequest>();

        group.MapPost("/scoring", CreateNewScoringFormatTest)
            .WithRequestValidation<InitNewTestRequest>();
        return group;
    }
    private async static Task<IResult> CreateNewGeneralFormatTest(
        HttpContext httpContext,
        ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<InitNewTestRequest>();
        var creator = httpContext.GetAuthenticatedUserId();
        var editors = request.GetParsedEditorIdsWithoutCreator(creator);

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
        var editors = request.GetParsedEditorIdsWithoutCreator(creator);

        InitScoringFormatTestCommand command = new(request.TestName, creator, editors);
        ErrOr<TestId> result = await mediator.Send(command);
        return CustomResults.FromErrOr(
            result,
            (id) => Results.Json(new NewTestInitializedResponse(id), statusCode: 201)
        );
    }
}
