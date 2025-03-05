using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestManagingService.Api.Contracts;
using TestManagingService.Api.Extensions;
using TestManagingService.Application.Tests.formats_shared.commands;

namespace TestManagingService.Api.Endpoints;

internal static class TestActionsHandlers
{
    internal static RouteGroupBuilder MapTestActionsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();

        group.MapPost("/suggestTags", AddTagSuggestionsForTest)
            .WithRequestValidation<SuggestTagsForTestRequest>();


        return group;
    }

    private static async Task<IResult> AddTagSuggestionsForTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        SuggestTagsForTestRequest request = httpContext.GetValidatedRequest<SuggestTagsForTestRequest>();

        AddTagSuggestionsForTestCommand command = new(testId, request.GetParsedTags());
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result, () => Results.Ok()
        );
    }
}