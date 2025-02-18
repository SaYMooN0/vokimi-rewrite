using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using SharedKernel.Configs;
using TestCatalogService.Api.Contracts.view_test.comments;
using TestCatalogService.Api.Contracts.view_test.comments.view_response;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.TestComments.commands.list;
using TestCatalogService.Application.Tests.formats_shared.commands;

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
        //     .AuthenticationRequired()
        //     .WithAccessCheckToCommentTest()
        //     .WithRequestValidation<NewTestCommentRequest>();
        // group.MapPost("/{commentId}/vote", VoteComment)
        //     .WithRequestValidation<>();
        // group.MapGet("/{commentId}/answers/{package}",ListCommentsAnswers )
        //     .WithRequestValidation<>();
        // group.MapPost("/{commentId}/answers/filtered/{package}",ListCommentAnswersFiltered )
        // //     .WithRequestValidation<>();     
        // group.MapPost("/{commentId}/answers/new", NewCommentAnswer)
        //     .AuthenticationRequired()
        //     .WithAccessCheckToCommentTest()
        //     .WithRequestValidation<NewTestCommentRequest>();

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

    private static async Task<IResult> ListCommentsFiltered(
        HttpContext httpContext,
        ISender mediator,
        JwtTokenConfig jwtTokenConfig,
        IDateTimeProvider dateTimeProvider,
        int package
    ) {
        var req = httpContext.GetValidatedRequest<ListCommentsFilteredRequest>();
        TestId testId = httpContext.GetTestIdFromRoute();
        AppUserId? viewerId = null;
        httpContext.ParseUserIdFromJwtToken(jwtTokenConfig).IsSuccess(out viewerId);
        var filter = req.ParseToFilter(dateTimeProvider, viewerId);
        if (filter.IsErr(out var filterErr)) {
            return CustomResults.ErrorResponse(filterErr);
        }

        ListFilteredTestCommentsCommand command = new(testId, filter.GetSuccess(), package, viewerId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (comments) => Results.Json(
                ViewTestCommentsListResponse.FromCommentsWithVotes(comments)
            )
        );
    }

  
}