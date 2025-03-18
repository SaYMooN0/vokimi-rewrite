using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.GeneralTestQuestions.commands.answers;

namespace TestCreationService.Api.Endpoints.general_format.answers;
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
    private static async Task<IResult> UpdateAnswer(
       HttpContext httpContext, ISender mediator
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
    private static async Task<IResult> RemoveAnswer(
       HttpContext httpContext, ISender mediator
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
