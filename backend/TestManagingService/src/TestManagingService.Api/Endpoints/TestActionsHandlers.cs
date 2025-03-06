using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestManagingService.Api.Contracts;
using TestManagingService.Api.Extensions;
using TestManagingService.Application.Tests.formats_shared.commands.tags;
using TestManagingService.Application.Tests.formats_shared.commands.tags.tag_suggestions;

namespace TestManagingService.Api.Endpoints;

internal static class TestActionsHandlers
{
    internal static RouteGroupBuilder MapTestActionsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();

        group.MapPost("/suggestTags", AddTagSuggestionsForTest)
            .WithRequestValidation<TestTagIdListRequest>();


        return group;
    }

    private static async Task<IResult> AddTagSuggestionsForTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TestTagIdListRequest tagIdListRequest = httpContext.GetValidatedRequest<TestTagIdListRequest>();

        AddTagSuggestionsForTestCommand command = new(testId, tagIdListRequest.GetParsedTags());
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result, () => Results.Ok()
        );
    }
}