using SharedKernel.Common.errors;

namespace TestCatalogService.Domain.Common;

public static class TestCatalogErrPresets
{
    public static Err CommentNotFound(TestCommentId testCommentId) => Err.ErrFactory.NotFound(
        "Unable to find the comment",
        details: $"Cannot find test comment with id {testCommentId}"
    );
}