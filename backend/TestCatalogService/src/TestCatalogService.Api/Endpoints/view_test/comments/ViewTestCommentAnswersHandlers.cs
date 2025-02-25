using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using SharedKernel.Configs;
using TestCatalogService.Api.Contracts.view_test.comments;
using TestCatalogService.Api.Contracts.view_test.comments.view_response;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.TestComments.commands.list_answers;
using TestCatalogService.Application.Tests.formats_shared.commands;
using TestCatalogService.Application.Tests.formats_shared.commands.comments;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Endpoints.view_test.comments;

internal static class ViewTestCommentAnswersHandlers
{
    internal static RouteGroupBuilder MapViewTestCommentAnswersHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired()
            .GroupCommentBelongsToTestCheckRequired();

        group.MapGet("/list/{package}", ListCommentsAnswers);
        group.MapPost("/listFiltered/{package}", ListCommentAnswersFiltered)
            .WithRequestValidation<ListCommentsFilteredRequest>();
        group.MapPost("/add", AddAnswerToComment)
            .AuthenticationRequired()
            .WithAccessCheckToCommentTest()
            .WithRequestValidation<NewTestCommentRequest>();

        return group;
    }

    private static async Task<IResult> ListCommentsAnswers(
        HttpContext httpContext,
        ISender mediator,
        JwtTokenConfig jwtTokenConfig,
        int package
    ) {
        TestCommentId commentId = httpContext.GetCommentIdFromRoute();
        AppUserId? viewerId = null;
        httpContext.ParseUserIdFromJwtToken(jwtTokenConfig).IsSuccess(out viewerId);

        ListCommentAnswersCommand command = new(commentId, package, viewerId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (comments) => Results.Json(
                ViewTestCommentsListResponse.FromCommentsWithVotes(comments)
            )
        );
    }

    private static async Task<IResult> ListCommentAnswersFiltered(
        HttpContext httpContext,
        ISender mediator,
        JwtTokenConfig jwtTokenConfig,
        IDateTimeProvider dateTimeProvider,
        int package
    ) {
        TestCommentId commentId = httpContext.GetCommentIdFromRoute();
        AppUserId? viewerId = null;
        httpContext.ParseUserIdFromJwtToken(jwtTokenConfig).IsSuccess(out viewerId);
        var req = httpContext.GetValidatedRequest<ListCommentsFilteredRequest>();

        var filter = req.ParseToFilter(dateTimeProvider, viewerId);
        if (filter.IsErr(out var filterErr)) {
            return CustomResults.ErrorResponse(filterErr);
        }

        ListFilteredCommentAnswersCommand command = new(commentId, filter.GetSuccess(), package, viewerId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (comments) => Results.Json(
                ViewTestCommentsListResponse.FromCommentsWithVotes(comments)
            )
        );
    }

    private static async Task<IResult> AddAnswerToComment(
        HttpContext httpContext,
        ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        AppUserId userId = httpContext.GetAuthenticatedUserId();
        TestCommentId parentCommentId = httpContext.GetCommentIdFromRoute();

        var request = httpContext.GetValidatedRequest<NewTestCommentRequest>();

        AddAnswerToTestCommentCommand command = new(
            parentCommentId, testId, userId, request.Text, request.ParsedAttachment(), request.MarkedAsSpoiler
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (newComment) => Results.Json(new {
                Comment = TestCommentViewDataResponse.FromComment(newComment, UserCommentVoteState.None)
            })
        );
    }
}