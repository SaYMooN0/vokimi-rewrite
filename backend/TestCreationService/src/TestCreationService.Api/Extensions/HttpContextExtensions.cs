using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Api.Extensions;

internal static class HttpContextExtensions
{
    private static Guid GetQuestionGuidFromRoute(this HttpContext context, string exceptionMessage) {
        var idStr = context.Request.RouteValues["questionId"]?.ToString() ?? "";
        if (!Guid.TryParse(idStr, out var questionGuid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData(exceptionMessage));
        }

        return questionGuid;
    }

    public static GeneralTestQuestionId GetGeneralTestQuestionIdFromRoute(this HttpContext context) =>
        new(context.GetQuestionGuidFromRoute(exceptionMessage: "Invalid general format test question id"));

    private static Guid GetAnswerGuidFromRoute(this HttpContext context, string exceptionMessage) {
        var idStr = context.Request.RouteValues["answerId"]?.ToString() ?? "";
        if (!Guid.TryParse(idStr, out var testGuid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData(exceptionMessage));
        }

        return testGuid;
    }

    public static GeneralTestAnswerId GetGeneralTestAnswerIdFromRoute(this HttpContext context) =>
        new(context.GetAnswerGuidFromRoute(exceptionMessage: "Invalid general format test answer id"));

    private static Guid GetResultGuidFromRoute(this HttpContext context, string exceptionMessage) {
        var idStr = context.Request.RouteValues["resultId"]?.ToString() ?? "";
        if (!Guid.TryParse(idStr, out var testGuid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData(exceptionMessage));
        }

        return testGuid;
    }

    public static GeneralTestResultId GetGeneralTestResultIdFromRoute(this HttpContext context) =>
        new(context.GetResultGuidFromRoute(exceptionMessage: "Invalid general format test result id"));

    public static TierListTestItemId GetItemIdFromRoute(this HttpContext context) {
        var idStr = context.Request.RouteValues["itemId"]?.ToString() ?? "";
        if (!Guid.TryParse(idStr, out var guid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData(
                "Incorrect tier list test item id provided",
                "Couldn't parse item id from route"
            ));
        }

        return new TierListTestItemId(guid);
    }
    public static TierListTestTierId GetTierIdFromRoute(this HttpContext context) {
        var idStr = context.Request.RouteValues["tierId"]?.ToString() ?? "";
        if (!Guid.TryParse(idStr, out var guid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData(
                "Incorrect tier list test tier id provided",
                "Couldn't parse tier id from route"
            ));
        }

        return new TierListTestTierId(guid);
    }
}