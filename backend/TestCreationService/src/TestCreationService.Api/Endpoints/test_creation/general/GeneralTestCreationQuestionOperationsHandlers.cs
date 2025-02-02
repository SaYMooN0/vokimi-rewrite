using ApiShared.extensions;
using ApiShared;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.GeneralTestQuestions.commands;
using TestCreationService.Application.Tests.general_format.commands.questions;
using SharedKernel.Common.domain;

namespace TestCreationService.Api.Endpoints.test_creation.general;

internal static class GeneralTestCreationQuestionOperationsHandlers

{
    internal static RouteGroupBuilder MapGeneralTestCreationQuestionOperationsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupCheckIfGeneralTestQuestionInProvidedTest()
            .GroupTestEditPermissionRequired();

        group.MapDelete("/delete", DeleteQuestion);

        group.MapPost("/update", UpdateQuestion)
            .WithRequestValidation<UpdateGeneralFormatTestQuestionRequest>();

        return group;
    }
    private async static Task<IResult> DeleteQuestion(
    HttpContext httpContext,
       ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();


        DeleteGeneralTestQuestionCommand command = new(testId, questionId);
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
            questionId,
            request.QuestionText,
            request.Images,
            request.CreateTimeLimit().GetSuccess(),
            request.CreateAnswerCountLimit().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrListOrNothing(
            result,
            () => Results.Ok()
        );
    }
}
