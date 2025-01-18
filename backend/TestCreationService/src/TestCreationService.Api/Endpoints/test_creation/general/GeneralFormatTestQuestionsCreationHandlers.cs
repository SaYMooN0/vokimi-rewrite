using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions.add_question;
using TestCreationService.Application.Tests.formats_shared.commands;
using TestCreationService.Application.Tests.general_format.commands.questions;

namespace TestCreationService.Api.Endpoints.test_creation.general;

internal static class GeneralFormatTestQuestionsCreationHandlers
{
    internal static RouteGroupBuilder MapGeneralFormatTestCreationHandlers(this RouteGroupBuilder group) {
        group.MapGet("/info", GetQuestionsInfo)
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapPost("/add", AddQuestion)
            .WithRequestValidation<AddGeneralFormatTestQuestionRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapDelete("/remove", RemoveQuestion)
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapPost("/update", UpdateQuestion)
            .WithRequestValidation<GeneralTestQuestionUpdateRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        group.MapPost("/updateOrder", UpdateQuestionsOrder) //map of order and question id
            .WithRequestValidation<GeneralTestQuestionsOrderUpdateRequest>()
            .AuthenticationRequired()
            .TestEditPermissionRequired();
        return group;
    }
    private async static Task<IResult> GetQuestionsInfo(
       HttpContext httpContext,
       ISender mediator
    ) {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
