using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;

namespace TestCatalogService.Api.Extensions;

internal static class HttpContextExtensions
{
    public static TestCommentId GetCommentIdFromRoute(this HttpContext context) {
        var idStr = context.Request.RouteValues["commentId"]?.ToString() ?? "";
        if (!Guid.TryParse(idStr, out var guid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData(
                "Incorrect comment id provided",
                "Couldn't parse comment id from route"
            ));
        }

        return new TestCommentId(guid);
    }
}