using ApiShared;
using ApiShared.extensions;
using MediatR;
using TestManagingService.Api.Contracts;
using TestManagingService.Api.Extensions;
using TestManagingService.Application.Tests.formats_shared.commands;

namespace TestManagingService.Api.Endpoints;

internal static class ManageTestOverallHandlers
{
    internal static RouteGroupBuilder MapManageTestOverallHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        group.MapPost("/updateInteractionsAccessSettings", UpdateTestInteractionsAccessSettings)
            .WithRequestValidation<UpdateTestInteractionsAccessSettingsRequest>();
        //get (TestFormat)

        //update interaction settings
        return group;
    }

    private static async Task<IResult> UpdateTestInteractionsAccessSettings(
        HttpContext httpContext, ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateTestInteractionsAccessSettingsRequest>();
        var testId = httpContext.GetTestIdFromRoute();

        UpdateTestInteractionsAccessSettingsCommand command = new(
            testId,
            TestAccessLevel: request.TestAccess,
            RatingsSetting: request.RatingsSetting,
            CommentsSetting: request.CommentsSetting,
            AllowTestTakenPosts: request.AllowTestTakenPosts,
            AllowTagSuggestions: request.AllowTagSuggestions
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrListOr(
            result, (settings) => Results.Json(new { TestInteractionsAccessSettings = settings })
        );
    }
}