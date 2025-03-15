using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.GeneralTestQuestions.commands.answers;

namespace TestCreationService.Api.Endpoints.test_creation.general_format;
internal static class GeneralTestCreationAnswersHandlers
{
    internal static RouteGroupBuilder MapGeneralTestCreationAnswersHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupCheckIfGeneralTestQuestionInProvidedTest()
            .GroupTestEditPermissionRequired();

        group.MapGet("/list", ListAnswers);
        group.MapPost("/add", AddAnswer)
            .WithRequestValidation<SaveGeneralTestAnswerRequest>();
        group.MapPost("/updateOrder", UpdateAnswersOrder)
            .WithRequestValidation<UpdateGeneralTestAnswersOrderRequest>();
        return group;
    }
    private static async Task<IResult> ListAnswers(
       HttpContext httpContext, ISender mediator
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
    private static async Task<IResult> AddAnswer(
       HttpContext httpContext, ISender mediator
    ) {
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveGeneralTestAnswerRequest>();

        AddAnswerToGeneralTestQuestionCommand command = new(
            questionId, 
            request.ParsedAnswerData().GetSuccess(),
            request.ParsedRelatedResultIds().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );

    }
    private static async Task<IResult> UpdateAnswersOrder(
       HttpContext httpContext, ISender mediator
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
