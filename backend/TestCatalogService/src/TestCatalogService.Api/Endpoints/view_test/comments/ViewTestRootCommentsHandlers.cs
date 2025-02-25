using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using SharedKernel.Configs;
using TestCatalogService.Api.Contracts.view_test.comments;
using TestCatalogService.Api.Contracts.view_test.comments.view_response;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.TestComments.commands.list_comments;
using TestCatalogService.Application.Tests.formats_shared.commands;
using TestCatalogService.Application.Tests.formats_shared.commands.comments;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Api.Endpoints.view_test.comments;

internal static class ViewTestRootCommentsHandlers
{
     internal static RouteGroupBuilder MapViewTestRootCommentsHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToViewTestRequired();

        group.MapGet("/list/{package}", ListComments);
        group.MapPost("/listFiltered/{package}", ListCommentsFiltered)
            .WithRequestValidation<ListCommentsFilteredRequest>();
        group.MapPost("/add", CreateNewComment)
            .AuthenticationRequired()
            .WithAccessCheckToCommentTest()
            .WithRequestValidation<NewTestCommentRequest>();
        
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

    private static async Task<IResult> CreateNewComment(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        AppUserId userId = httpContext.GetAuthenticatedUserId();
        var request = httpContext.GetValidatedRequest<NewTestCommentRequest>();

        AddTestCommentCommand command = new(
            testId, userId, request.Text, request.ParsedAttachment(), request.MarkedAsSpoiler
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result,
            (newComment) => Results.Json(new {
                Comment = TestCommentViewDataResponse.FromComment(newComment, UserCommentVoteState.None)
            })
        );
    }
}