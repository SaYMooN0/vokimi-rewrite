using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;
using TestCreationService.Application.Tests.general_format.commands.questions;

namespace TestCreationService.Api.Endpoints.test_creation.general;

internal static class GeneralFormatTestQuestionsCreationHandlers
{
    internal static RouteGroupBuilder MapGeneralFormatTestCreationQuestionsHandlers(this RouteGroupBuilder group) {
        group.MapGet("/", ListQuestions)
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapPost("/add", AddQuestion)
            .WithRequestValidation<AddGeneralFormatTestQuestionRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapDelete("/remove", RemoveQuestion)
            .WithRequestValidation<RemoveGeneralFormatTestQuestionRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapPost("/update", UpdateQuestion)
            //.WithRequestValidation<GeneralTestQuestionUpdateRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapPost("/updateOrder", UpdateQuestionsOrder)
            //.WithRequestValidation<UpdateGeneralTestQuestionsOrderRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        //map of order and question id
        return group;
    }
    private async static Task<IResult> ListQuestions(
       HttpContext httpContext,
       ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();

        ListGeneralTestQuestionsWithOrderCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (questions) => Results.Json(new {
                Questions = questions.Select(
                    qWithO => GeneralFormatTestQuestionInfoResponse.FromQuestion(qWithO.Value, qWithO.Key)
                )
            })
        );
    }
    private async static Task<IResult> AddQuestion(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<AddGeneralFormatTestQuestionRequest>();
        TestId testId = httpContext.GetTestIdFromRoute();

        AddGeneralTestQuestionCommand command = new(testId, request.AnswersType);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
    private async static Task<IResult> RemoveQuestion(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<RemoveGeneralFormatTestQuestionRequest>();
        TestId testId = httpContext.GetTestIdFromRoute();
        GeneralTestQuestionId questionId = request.ParsedQuestionId;

        RemoveGeneralTestQuestionCommand command = new(testId, questionId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
    private async static Task<IResult> UpdateQuestion(
       HttpContext httpContext,
       ISender mediator
    ) {
        throw new NotImplementedException();
    }
    private async static Task<IResult> UpdateQuestionsOrder(
       HttpContext httpContext,
       ISender mediator
    ) {
        throw new NotImplementedException();
    }
}
