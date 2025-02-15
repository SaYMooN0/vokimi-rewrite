using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace TestTakingService.Domain.Common;

public class TestTakenRecordId : EntityId
{
    public TestTakenRecordId(Guid value) : base(value) { }
    public static TestTakenRecordId CreateNew() => new(Guid.CreateVersion7());
}

public class GeneralTestTakenRecordQuestionDetailsId : EntityId
{
    public GeneralTestTakenRecordQuestionDetailsId(Guid value) : base(value) { }
    public static GeneralTestTakenRecordQuestionDetailsId CreateNew() => new(Guid.CreateVersion7());
}
public class TestFeedbackRecordId : EntityId
{
    public TestFeedbackRecordId(Guid value) : base(value) { }
    public static TestFeedbackRecordId CreateNew() => new(Guid.CreateVersion7());
}