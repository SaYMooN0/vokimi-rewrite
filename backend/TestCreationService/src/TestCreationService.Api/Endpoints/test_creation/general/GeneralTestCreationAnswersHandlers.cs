using ApiShared.extensions;
using ApiShared;
using MediatR;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Extensions;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Endpoints.test_creation.general;
internal static class GeneralTestCreationAnswersHandlers
{
    internal static RouteGroupBuilder MapGeneralTestCreationAnswersHandlers(this RouteGroupBuilder group) {
        group.MapGet("/list", ListAnswers)
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapPost("/add", AddAnswer)
            .WithRequestValidation<AddGeneralFormatTestAnswerRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapPost("/updateOrder", UpdateAnswersOrder)
            .WithRequestValidation<UpdateGeneralTestAnswersOrderRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        return group;
    }
    private async static Task<IResult> ListAnswers(
       HttpContext httpContext,
       ISender mediator
    ) {
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();

        // command = new();
        //var result = await mediator.Send(command);
        return CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented());
    }
    private async static Task<IResult> AddAnswer(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<AddGeneralFormatTestAnswerRequest>();
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();


        // command = new();
        //var result = await mediator.Send(command);
        return CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented());
    }
    private async static Task<IResult> UpdateAnswersOrder(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateGeneralTestAnswersOrderRequest>();
        GeneralTestQuestionId questionId = httpContext.GetGeneralTestQuestionIdFromRoute();


        // command = new();
        //var result = await mediator.Send(command);
        return CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented());
    }
}
