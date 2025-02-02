using ApiShared.extensions;
using ApiShared;
using MediatR;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;
using SharedKernel.Common.errors;
using TestCreationService.Application.Tests.formats_shared.commands;
using TestCreationService.Api.Contracts.Tests.test_creation.formats_shared.update_editors;
using TestCreationService.Api.Extensions;
using SharedKernel.Common.domain;

namespace TestCreationService.Api.Endpoints.test_creation.formats_shared;

internal static class FormatsSharedTestCreationHandlers
{
    internal static RouteGroupBuilder MapFormatsSharedTestCreationHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired();

        group.MapPost("/updateEditors", UpdateTestEditors)
            .WithRequestValidation<UpdateTestEditorsRequest>()
            .OnlyByTestCreator();
        group.MapPost("/changeCreator", ChangeTestCreator)
            .WithRequestValidation<ChangeTestCreatorRequest>()
            .OnlyByTestCreator();
        group.MapPost("/deleteTest", DeleteTest)
            .OnlyByTestCreator();

        group.MapPost("/updateMainInfo", UpdateTestMainInfo)
            .WithRequestValidation<UpdateTestMainInfoRequest>()
            .TestEditPermissionRequired();
        group.MapPost("/updateInteractionsAccessSettings", UpdateTestInteractionsAccessSettings)
            .WithRequestValidation<UpdateTestInteractionsAccessSettingsRequest>()
            .TestEditPermissionRequired();
        group.MapPost("/updateCover", UpdateTestCover)
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
    private async static Task<IResult> ChangeTestCreator(
       HttpContext httpContext,
       ISender mediator
    ) {
        var request = httpContext.GetValidatedRequest<ChangeTestCreatorRequest>();
        var creator = httpContext.GetAuthenticatedUserId();

        ChangeTestCreatorCommand command = new(
            httpContext.GetTestIdFromRoute(),
            request.ParsedNewCreatorId().GetSuccess(),
            request.KeepCurrentCreatorAsEditor
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
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
