using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.exceptions;

namespace TestCreationService.Api.Extensions;

internal static class HttpContextExtensions
{
    private static Guid GetQuestionGuidFromRoute(this HttpContext context, string exceptionMessage) {
        var idStr = context.Request.RouteValues["questionId"]?.ToString() ?? "";
        if (!Guid.TryParse(idStr, out var testGuid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData(exceptionMessage));
        }

        return testGuid;
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
}
