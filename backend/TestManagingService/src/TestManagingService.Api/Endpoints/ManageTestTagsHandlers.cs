using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestManagingService.Api.Contracts;
using TestManagingService.Api.Contracts.tags;
using TestManagingService.Api.Extensions;
using TestManagingService.Application.Tests.formats_shared.commands.tags;
using TestManagingService.Application.Tests.formats_shared.commands.tags.tag_suggestions;

namespace TestManagingService.Api.Endpoints;

internal static class ManageTestTagsHandlers
{
    internal static RouteGroupBuilder MapManageTestTagsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        group.MapGet("/list", ListTestTags);
        group.MapPost("/update", UpdateTagsForTest)
            .WithRequestValidation<TestTagIdListRequest>();


        group.MapGet("/listTagSuggestions", ListTagSuggestionsForTest);        //list filtered
        group.MapPost("/acceptTagSuggestions", AcceptTagSuggestionsForTest)
            .WithRequestValidation<TestTagIdListRequest>();
        group.MapPost("/declineTagSuggestions", DeclineTagSuggestionsForTest)
            .WithRequestValidation<TestTagIdListRequest>();
        group.MapPost("/declineAndBanTagSuggestions", DeclineAndBanTagSuggestionsForTest)
            .WithRequestValidation<TestTagIdListRequest>();
        return group;
    }
    private static async Task<IResult> UpdateTagsForTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TestTagIdListRequest tagIdListRequest = httpContext.GetValidatedRequest<TestTagIdListRequest>();

        UpdateTagsForTestCommand command = new(testId, tagIdListRequest.GetParsedTags());
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
    private static async Task<IResult> ListTestTags(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();

        ListTestTagsCommand command = new(testId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (tags) => Results.Json(new {
                Tags = tags.Select(t=>t.Value)
            })
        );
    }
    private static async Task<IResult> ListTagSuggestionsForTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TestTagIdListRequest tagIdListRequest = httpContext.GetValidatedRequest<TestTagIdListRequest>();

        ListTagSuggestionsForTestCommand command = new(testId, tagIdListRequest.GetParsedTags());
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (suggestions) => Results.Json(new {
                TagSuggestions = suggestions.Select(TestTagSuggestionViewResponse.FromSuggestion)
            })
        );
    }
    private static async Task<IResult> AcceptTagSuggestionsForTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TestTagIdListRequest tagIdListRequest = httpContext.GetValidatedRequest<TestTagIdListRequest>();

        AcceptTagSuggestionsForTestCommand command = new(testId, tagIdListRequest.GetParsedTags());
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }

    private static async Task<IResult> DeclineTagSuggestionsForTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TestTagIdListRequest tagIdListRequest = httpContext.GetValidatedRequest<TestTagIdListRequest>();

        DeclineTagSuggestionsForTestCommand command = new(testId, tagIdListRequest.GetParsedTags());
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }

    private static async Task<IResult> DeclineAndBanTagSuggestionsForTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TestTagIdListRequest tagIdListRequest = httpContext.GetValidatedRequest<TestTagIdListRequest>();

        DeclineAndBanTagSuggestionsForTestCommand command = new(testId, tagIdListRequest.GetParsedTags());
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}