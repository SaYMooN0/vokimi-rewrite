using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Api.Extensions;

internal static class HttpContextExtensions
{
    public static ErrOr<TestCommentId> GetCommentTestIdFromRoute(this HttpContext context) {
        var idStr = context.Request.RouteValues["commentId"]?.ToString() ?? "";
        if (!Guid.TryParse(idStr, out var guid)) {
            return new Err("Incorrect comment id provided");
        }

        return new TestCommentId(guid);
    }
}