using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Api.Contracts.view_test.comments;
using TestCatalogService.Api.Contracts.view_test.comments.edit_comment;
using TestCatalogService.Api.Extensions;
using TestCatalogService.Application.TestComments.commands;
using TestCatalogService.Application.Tests.formats_shared.commands.comments;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Api.Endpoints.view_test.comments;

internal static class ViewTestCommentActionsHandlers
{
    internal static RouteGroupBuilder MapViewTestCommentActionsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToViewTestRequired()
            .GroupCommentBelongsToTestCheckRequired();

        group.MapPost("/vote/{isUp}", VoteComment);
        group.MapPost("/markAsSpoiler", MarkCommentAsSpoiler);
        group.MapPost("/report", ReportComment)
            .WithRequestValidation<ReportTestCommentRequest>();

        group.MapDelete("/delete", DeleteComment);
        group.MapPost("/edit", EditComment)
            .WithAccessCheckToCommentTest()
            .WithRequestValidation<EditTestCommentRequest>();

        return group;
    }

    private static async Task<IResult> VoteComment(
        HttpContext httpContext, ISender mediator, bool isUp
    ) {
        TestCommentId commentId = httpContext.GetCommentIdFromRoute();
        AppUserId userId = httpContext.GetAuthenticatedUserId();


        VoteForTestCommentCommand command = new(commentId, userId, isUp);
        var result = await mediator.Send(command);
        return CustomResults.FromErrOr(result,
            (state) => Results.Json(new { VoteState = state })
        );
    }

    private static async Task<IResult> MarkCommentAsSpoiler(
        HttpContext httpContext, ISender mediator
    ) {
        TestCommentId commentId = httpContext.GetCommentIdFromRoute();
        AppUserId userId = httpContext.GetAuthenticatedUserId();


        MarkCommentAsSpoilerCommand command = new(commentId, userId);
        var result = await mediator.Send(command);
        return CustomResults.FromErrOrNothing(result,
            () => Results.Ok()
        );
    }

    private static async Task<IResult> ReportComment(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TestCommentId commentId = httpContext.GetCommentIdFromRoute();
        AppUserId userId = httpContext.GetAuthenticatedUserId();
        var request = httpContext.GetValidatedRequest<ReportTestCommentRequest>();

        ReportTestCommentCommand command = new(testId, commentId, userId, request.Text, request.Reason);
        var result = await mediator.Send(command);
        return CustomResults.FromErrOrNothing(result,
            () => Results.Ok()
        );
    }

    private static async Task<IResult> DeleteComment(
        HttpContext httpContext, ISender mediator
    ) {
        TestCommentId commentId = httpContext.GetCommentIdFromRoute();
        AppUserId userId = httpContext.GetAuthenticatedUserId();


        DeleteTestCommentCommand command = new(commentId, userId);
        var result = await mediator.Send(command);
        return CustomResults.FromErrOr(result,
            (comment) => Results.Json(TestCommentDeletedResponse.FromComment(comment))
        );
    }

    private static async Task<IResult> EditComment(
        HttpContext httpContext, ISender mediator
    ) {
        TestCommentId commentId = httpContext.GetCommentIdFromRoute();
        AppUserId userId = httpContext.GetAuthenticatedUserId();
        var request = httpContext.GetValidatedRequest<EditTestCommentRequest>();


        EditTestCommentCommand command = new(commentId, userId, request.NewText);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result, (comment) => Results.Json(new {
                NewCommentText = comment.Text
            })
        );
    }
}