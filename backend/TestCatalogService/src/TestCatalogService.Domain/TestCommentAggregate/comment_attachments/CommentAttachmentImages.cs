using System.Collections.Immutable;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Rules;

namespace TestCatalogService.Domain.TestCommentAggregate.comment_attachments;

public class CommentAttachmentImages : TestCommentAttachment
{
    public ImmutableArray<string> Images { get; private init; }

    public CommentAttachmentImages(ImmutableArray<string> images) {
        Images = images;
    }

    public override CommentAttachmentType RelatedEnumType => CommentAttachmentType.Images;

    public override ErrOrNothing CheckForErr() {
        if (Images.IsEmpty) {
            return new Err("Images list is empty");
        }

        foreach (var path in Images) {
            if (TestCommentRules.CheckAttachmentImagePathForErrs(path).IsErr(out var err)) {
                return err;
            }
        }

        return ErrOrNothing.Nothing;
    }

    public override string ToStorageString() => string.Join(',', Images);

    public static CommentAttachmentImages CreateFromStorageString(string imagesString) =>
        new(imagesString.Split(",").ToImmutableArray());

    public override IEnumerable<object> GetEqualityComponents() {
        yield return RelatedEnumType;
        foreach (var i in Images) {
            yield return i;
        }
    }
}