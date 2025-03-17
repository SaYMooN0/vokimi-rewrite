using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.tier_list_format.commands.items;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Api.Endpoints.test_creation.tier_list_format;

internal static class TierListTestCreationItemOperationsHandlers
{
    internal static RouteGroupBuilder MapTierListTestCreationItemOperationsHandlers(
        this RouteGroupBuilder group
    ) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired();

        group.MapPost("/update", UpdateTestItem)
            .WithRequestValidation<SaveItemForTierListTestRequest>();
        group.MapDelete("/remove", RemoveItemFromTest);


        return group;
    }

    private static async Task<IResult> UpdateTestItem(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TierListTestItemId itemId = httpContext.GetItemIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveItemForTierListTestRequest>();

        UpdateTierListTestItemCommand command = new(
            testId,
            itemId,
            request.ItemName,
            request.ItemClarification,
            request.ParsedItemContentData().GetSuccess()
        );
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(result, (newItem) => Results.Json(new {
                TierListTestItems = TierListTestItemInfoResponse.FromItem(newItem)
            })
        );
    }
    private static async Task<IResult> RemoveItemFromTest(
        HttpContext httpContext, ISender mediator
    ) {
        TestId testId = httpContext.GetTestIdFromRoute();
        TierListTestItemId itemId = httpContext.GetItemIdFromRoute();

        RemoveTierListTestItemCommand command = new(testId, itemId);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}