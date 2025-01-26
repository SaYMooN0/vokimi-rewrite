using ApiShared.extensions;
using ApiShared;
using MediatR;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Extensions;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;
using TestCreationService.Application.GeneralTestQuestions.commands.answers;

namespace TestCreationService.Api.Endpoints.test_creation.general;
internal static class GeneralTestCreationAnswerOperationsHandlers
{
    internal static RouteGroupBuilder MapGeneralTestCreationAnswerOperationsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupCheckIfGeneralTestQuestionInProvidedTest()
            .GroupTestEditPermissionRequired();

        group.MapPost("/update", UpdateAnswer)
            .WithRequestValidation<SaveGeneralTestAnswerRequest>();
        group.MapDelete("/remove", RemoveAnswer);


        return group;
    }
    private async static Task<IResult> UpdateAnswer(
       HttpContext httpContext,
       ISender mediator
    ) {
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();
        GeneralTestAnswerId answerId = httpContext.GetGeneralTestAnswerIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveGeneralTestAnswerRequest>();

        UpdateAnswerForGeneralTestQuestionCommand command = new(
            questionId,
            answerId,
            request.ParsedAnswerData().GetSuccess(),
            request.ParsedRelatedResultIds().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
    private async static Task<IResult> RemoveAnswer(
       HttpContext httpContext,
       ISender mediator
    ) {
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();
        GeneralTestAnswerId answerId = httpContext.GetGeneralTestAnswerIdFromRoute();

        RemoveAnswerFromGeneralTestQuestionCommand command = new(
            questionId,
            answerId
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
}
