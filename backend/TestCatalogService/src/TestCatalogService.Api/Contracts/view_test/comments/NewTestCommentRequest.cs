using System.Text.Json;
using ApiShared.interfaces;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Rules;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Domain.TestCommentAggregate.comment_attachments;

namespace TestCatalogService.Api.Contracts.view_test.comments;

public class NewTestCommentRequest : IRequestWithValidationNeeded
{
    public CommentAttachmentType? AttachmentType { get; init; }
    public string? AttachmentJsonString { get; init; }
    public string Text { get; init; }
    public bool MarkedAsSpoiler { get; init; } 

    public RequestValidationResult Validate() {
        if (TestCommentRules.CheckCommentTextForErrs(Text).IsErr(out var err)) {
            return err;
        }

        if (AttachmentType is not null) {
            if (ParseCommentAttachment(AttachmentType.Value, AttachmentJsonString).IsErr(out err)) {
                return err;
            }
        }

        return RequestValidationResult.Success;
    }

    private static ErrOr<TestCommentAttachment> ParseCommentAttachment(
        CommentAttachmentType attachmentType, string? json
    ) {
        if (string.IsNullOrWhiteSpace(json)) {
            return Err.ErrFactory.InvalidData("Comment attachment data is empty, but attachment type is provided");
        }

        try {
            TestCommentAttachment? attachment = attachmentType switch {
                CommentAttachmentType.Images =>
                    JsonSerializer.Deserialize<CommentAttachmentImages>(json),
                CommentAttachmentType.GeneralTestResult =>
                    JsonSerializer.Deserialize<CommentAttachmentGeneralTestResult>(json),
                _ => throw new ArgumentException()
            };
            if (attachment is null) {
                return new Err("Unable to correctly parse comment attachment data");
            }

            return attachment;
            return attachment;
        }
        catch (ArgumentException) {
            return new Err($"Unknown attachment type: {attachmentType}");
        }
        catch (JsonException ex) {
            return new Err($"Invalid attachment data content", details: "Invalid json");
        }
        catch {
            return new Err($"Unknown error");
        }
    }
}