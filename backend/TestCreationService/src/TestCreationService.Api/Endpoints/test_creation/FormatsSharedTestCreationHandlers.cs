using ApiShared.extensions;
using ApiShared;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;
using SharedKernel.Common.errors;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Contracts.Tests.test_initialization;
using TestCreationService.Application.Tests.general_format;
using TestCreationService.Application.Tests.formats_shared.commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared.update_editors;

namespace TestCreationService.Api.Endpoints.test_creation;

internal static class FormatsSharedTestCreationHandlers
{
    internal static RouteGroupBuilder MapFormatsSharedTestCreationHandlers(this RouteGroupBuilder group) {
        group.MapPost("/updateEditors", UpdateTestEditors)
            .WithRequestValidation<UpdateTestEditorsRequest>()
            .AuthenticationRequired()
            .OnlyByTestCreator();
        group.MapPost("/deleteTest", DeleteTest)
            .AuthenticationRequired()
            .OnlyByTestCreator();
        group.MapPost("/updateMainInfo", UpdateTestMainInfo)
            .AuthenticationRequired()
            .WithRequestValidation<UpdateTestMainInfoRequest>()
            .TestEditPermissionRequired();
        group.MapPost("/updateInteractionsAccessSettings", UpdateTestInteractionsAccessSettings)
            .AuthenticationRequired()
            .WithRequestValidation<UpdateTestInteractionsAccessSettingsRequest>()
            .TestEditPermissionRequired();
        group.MapPost("/updateCover", UpdateTestCover)
            .AuthenticationRequired()
            .WithRequestValidation<UpdateTestCoverRequest>()
            .TestEditPermissionRequired();
        return group;
    }
    private async static Task<IResult> UpdateTestEditors(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateTestEditorsRequest>();
        var creator = httpContext.GetAuthenticatedUserId();
        var editors = request.EditorIds
            .Select(id => new AppUserId(new(id)))
            .Where(id => id != creator)
            .ToHashSet();

        UpdateTestEditorsCommand command = new(httpContext.GetTestIdFromRoute(), editors);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (ids) => Results.Json(new TestEditorsUpdatedResponse(ids), statusCode: 200)
        );
    }
    private async static Task<IResult> DeleteTest(
       HttpContext httpContext,
       ISender mediator
    ) {
        DeleteTestCommand command = new(httpContext.GetTestIdFromRoute());
        ErrOrNothing result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
    private async static Task<IResult> UpdateTestMainInfo(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateTestMainInfoRequest>();
        var testId = httpContext.GetTestIdFromRoute();

        UpdateTestMainInfoCommand command = new(testId, request.TestName, request.Description, request.Language);
        ErrOrNothing result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }
    private async static Task<IResult> UpdateTestInteractionsAccessSettings(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateTestInteractionsAccessSettingsRequest>();
        var testId = httpContext.GetTestIdFromRoute();

        UpdateTestInteractionsAccessSettingsCommand command = new(
            testId,
            TestAccessLevel: request.TestAccess,
            RatingsSetting: request.RatingsSetting,
            DiscussionsSetting: request.DiscussionsSetting,
            AllowTestTakenPosts: request.AllowTestTakenPosts,
            TagSuggestionsSetting: request.TagSuggestionsSetting
        );
        ErrListOrNothing result = await mediator.Send(command);

        return CustomResults.FromErrListOrNothing(
            result,
            () => Results.Ok()
        );
    }
    private async static Task<IResult> UpdateTestCover(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<UpdateTestCoverRequest>();
        var testId = httpContext.GetTestIdFromRoute();

        UpdateTestCoverCommand command = new(testId, request.CoverImg);
        ErrOrNothing result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result, () => Results.Ok()
        );
    }
}
