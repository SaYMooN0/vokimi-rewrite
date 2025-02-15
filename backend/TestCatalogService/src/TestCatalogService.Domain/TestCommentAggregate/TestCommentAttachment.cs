using SharedKernel.Common.domain.value_object;
using SharedKernel.Common.errors;

namespace TestCatalogService.Domain.TestCommentAggregate;

public abstract class TestCommentAttachment : ValueObject
{
    public abstract CommentAttachmentType RelatedEnumType { get; }
    public abstract ErrOrNothing CheckForErr();
    public abstract string ToStorageString();
}