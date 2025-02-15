using System.Collections.Immutable;
using SharedKernel.Common.errors;

namespace TestCatalogService.Domain.TestCommentAggregate.comment_attachments;

public class CommentAttachmentImages : TestCommentAttachment
{
    public ImmutableArray<string> Images { get; private init; }

    private CommentAttachmentImages(ImmutableArray<string> images) {
        Images = images;
    }

    public override CommentAttachmentType RelatedEnumType => CommentAttachmentType.Images;

    public override ErrOrNothing CheckForErr() => Images.Length  == 0 ? new Err("ds") : ErrOrNothing.Nothing;

    public override string ToStorageString() => ;

    public override IEnumerable<object> GetEqualityComponents() {
        yield return RelatedEnumType;
    }
}