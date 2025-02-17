using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;

namespace TestCatalogService.Domain.TestCommentAggregate.comment_attachments;

public class CommentAttachmentGeneralTestResult : TestCommentAttachment
{
    public GeneralTestResultId ResultId { get; private init; }

    public CommentAttachmentGeneralTestResult(GeneralTestResultId resultId) {
        ResultId = resultId;
    }

    public override CommentAttachmentType RelatedEnumType => CommentAttachmentType.GeneralTestResult;

    public override ErrOrNothing CheckForErr() =>
        ResultId is null ? new Err("Result Id is not set") : ErrOrNothing.Nothing;

    public override string ToStorageString() => ResultId.ToString();

    public static CommentAttachmentGeneralTestResult CreateFromStorageString(string resultId) =>
        new(new(new Guid(resultId)));

    public override IEnumerable<object> GetEqualityComponents() {
        yield return RelatedEnumType;
        yield return ResultId;
    }
}