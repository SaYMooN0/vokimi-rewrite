using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Domain.TestCommentAggregate.comment_attachments;

namespace TestCatalogService.Infrastructure.Persistence.configurations.value_converters;

public class TestCommentAttachmentConverter : ValueConverter<TestCommentAttachment?, string?>
{
    public TestCommentAttachmentConverter()
        : base(
            attachment => ToStr(attachment),
            str => FromStr(str)
        ) { }

    private static TestCommentAttachment? FromStr(string? str) {
        if (str is null) {
            return null;
        }

        var split = str.Split(':', 2);
        var matchingEnumType = Enum.Parse<CommentAttachmentType>(split[0]);
        
        string content = split[1];
        TestCommentAttachment creationRes = matchingEnumType switch {
            CommentAttachmentType.Images => CommentAttachmentImages.CreateFromStorageString(content),
            CommentAttachmentType.GeneralTestResult =>
                CommentAttachmentGeneralTestResult.CreateFromStorageString(content),
            _ => throw new ArgumentException(
                $"Incorrect CommentAttachmentType '{matchingEnumType}' in the {nameof(FromStr)}"
            )
        };
        if (creationRes.CheckForErr().IsErr(out var err)) {
            throw new ErrCausedException(err);
        }
        return creationRes;
    }

    private static string? ToStr(TestCommentAttachment? attachment) {
        if (attachment is null) {
            return null;
        }

        return attachment.RelatedEnumType.ToString() + ':' + attachment.ToStorageString();
    }
}