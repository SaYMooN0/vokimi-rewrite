using ApiShared.extensions;
using ApiShared;
using MediatR;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Extensions;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;
using SharedKernel.Common.errors;
using TestCreationService.Application.Tests.general_format.commands.answers;

namespace TestCreationService.Api.Endpoints.test_creation.general;
internal static class GeneralTestCreationAnswersHandlers
{
    internal static RouteGroupBuilder MapGeneralTestCreationAnswersHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired()
            .GroupCheckIfGeneralTestQuestionInProvidedTest();

        group.MapGet("/list", ListAnswers);
        group.MapPost("/add", AddAnswer)
            .WithRequestValidation<SaveGeneralTestAnswerRequest>();
        group.MapPost("/updateOrder", UpdateAnswersOrder)
            .WithRequestValidation<UpdateGeneralTestAnswersOrderRequest>();
        return group;
    }
    private async static Task<IResult> ListAnswers(
       HttpContext httpContext,
       ISender mediator
    ) {
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();

        ListGeneralTestAnswersWithOrderCommand command = new(questionId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (answers) => Results.Json(new {
                Answers = answers.Select(
                    i => GeneralFormatTestAnswerInfoResponse.Create(i.Answer, i.Order)
                )
            })
        );
    }
    private async static Task<IResult> AddAnswer(
       HttpContext httpContext,
       ISender mediator
    ) {
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveGeneralTestAnswerRequest>();

        AddAnswerToGeneralTestQuestionCommand command = new(questionId, request.ParsedAnswerData().GetSuccess());
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );

    }
    private async static Task<IResult> UpdateAnswersOrder(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateGeneralTestAnswersOrderRequest>();
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();

        UpdateAnswersOrderInGeneralTestQuestionCommand command = new(
            questionId,
            request.CreateOrderController().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
}
