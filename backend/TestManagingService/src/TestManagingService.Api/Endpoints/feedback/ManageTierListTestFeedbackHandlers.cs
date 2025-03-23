using ApiShared.extensions;
using TestManagingService.Api.Extensions;

namespace TestManagingService.Api.Endpoints.feedback;

internal static class ManageTierListTestFeedbackHandlers
{
    internal static RouteGroupBuilder MapManageTierListTestFeedbackHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupUserAccessToManageTestRequired();

        group.MapGet("/list", ListFeedbackForTierListTest);
        group.MapPost("/list/filtered", ListFilteredFeedbackForTierListTest)
            .WithRequestValidation<ListFilteredTierListTestFeedbackRecordsRequest>();

        group.MapPost("/disable", DisableFeedbackForTierListTest);
        group.MapPost("/enable", EnableFeedbackForTierListTest)
            .WithRequestValidation<EnableTierListTestFeedbackOptionRequest>();

        return group;
    }
}