using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;

namespace SharedKernel.IntegrationEvents.test_managing;

public record class TestCommentReportedIntegrationEvent(
    TestId TestId,
    AppUserId ReportAuthorId,
    TestCommentId CommentId,
    string Text,
    CommentReportReason Reason
) : IIntegrationEvent;