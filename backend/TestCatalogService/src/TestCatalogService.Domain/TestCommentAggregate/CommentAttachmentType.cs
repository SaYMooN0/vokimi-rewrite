using SharedKernel.Common.tests;
using TestCatalogService.Domain.TestCommentAggregate.comment_attachments;

namespace TestCatalogService.Domain.TestCommentAggregate;

public enum CommentAttachmentType
{
    Images,
    GeneralTestResult
}

public static class CommentAttachmentTypeExtensions
{

    public static bool IsPossibleForTestFormat(this CommentAttachmentType attachmentType, TestFormat format) =>
        attachmentType switch {
            CommentAttachmentType.Images => true,
            CommentAttachmentType.GeneralTestResult => format == TestFormat.General,
            _ => throw new ArgumentException(
                $"Incorrect CommentAttachmentType '{attachmentType}' in the {nameof(IsPossibleForTestFormat)}"
            )
        };
}