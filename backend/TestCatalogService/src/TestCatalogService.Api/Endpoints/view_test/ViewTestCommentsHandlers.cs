using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Configs;
using TestCatalogService.Api.Contracts.view_test.comments;
using TestCatalogService.Api.Contracts.view_test.comments.view_response;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.TestComments.commands.list;

namespace TestCatalogService.Api.Endpoints.view_test;

internal static class ViewTestCommentsHandlers
{
    internal static RouteGroupBuilder MapViewTestCommentsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();
        group.MapGet("/{package}", ListComments);
        group.MapPost("/filtered/{package}", ListCommentsFiltered)
            .WithRequestValidation<ListCommentsFilteredRequest>();
        // group.MapPost("/new", CreateNewComment)
        //     .WithRequestValidation<NewTestCommentRequest>();
        // group.MapPost("/{commentId}/vote", VoteComment)
        //     .WithRequestValidation<>();
        // group.MapGet("/{commentId}/answers/{package}",ListCommentsAnswers )
        //     .WithRequestValidation<>();
        // group.MapPost("/{commentId}/answers/filtered/{package}",ListCommentAnswersFiltered )
        //     .WithRequestValidation<>();     
        // group.MapPost("/{commentId}/answers/new",NewCommentAnswer )
        //     .WithRequestValidation<>();

        //mark as spoiler
        return group;
    }

    private static async Task<IResult> ListComments(
        HttpContext httpContext,
        ISender mediator,
        JwtTokenConfig jwtTokenConfig,
        int package
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        AppUserId? viewerId = null;
        httpContext.ParseUserIdFromJwtToken(jwtTokenConfig).IsSuccess(out viewerId);

        ListTestCommentsCommand command = new(testId, package, viewerId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (comments) => Results.Json(
                ViewTestCommentsListResponse.FromCommentsWithVotes(comments)
            )
        );
    }

    private static Task ListCommentsFiltered(
        HttpContext context,
        ISender mediator,
        int package
    ) { }
}