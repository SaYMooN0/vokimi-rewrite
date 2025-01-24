using ApiShared.extensions;
using ApiShared;
using MediatR;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;
using TestCreationService.Application.Tests.general_format.commands.questions;
using TestCreationService.Api.Extensions;

namespace TestCreationService.Api.Endpoints.test_creation.general;

internal static class GeneralTestCreationQuestionOperationsHandlers

{
    internal static RouteGroupBuilder MapGeneralTestCreationQuestionOperationsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired()
            .GroupCheckIfGeneralTestQuestionInProvidedTest();

        group.MapDelete("/remove", RemoveQuestion);

        group.MapPost("/update", UpdateQuestion)
            .WithRequestValidation<UpdateGeneralFormatTestQuestionRequest>();

        return group;
    }
    private async static Task<IResult> RemoveQuestion(
    HttpContext httpContext,
       ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();

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
        TestId testId = httpContext.GetTestIdFromRoute();
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateGeneralFormatTestQuestionRequest>();

        UpdateGeneralTestQuestionCommand command = new(
            testId,
            questionId,
            request.QuestionText,
            request.Images,
            request.CreateTimeLimit().GetSuccess(),
            request.CreateAnswerCountLimit().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
}
