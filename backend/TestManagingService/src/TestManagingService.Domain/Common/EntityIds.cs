using SharedKernel.Common.domain.entity;

namespace TestManagingService.Domain.Common;


public class TestCommentReportId : EntityId
{
    public TestCommentReportId(Guid value) : base(value) { }

    public static TestCommentReportId CreateNew() => new(Guid.CreateVersion7());
}

public class TestFeedbackRecordId : EntityId
{
    public TestFeedbackRecordId(Guid value) : base(value) { }
    public static TestFeedbackRecordId CreateNew() => new(Guid.CreateVersion7());
}
public class TagSuggestionForTestId : EntityId
{
    public TagSuggestionForTestId(Guid value) : base(value) { }
    public static TagSuggestionForTestId CreateNew() => new(Guid.CreateVersion7());
}