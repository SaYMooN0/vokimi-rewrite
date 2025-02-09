using SharedKernel.Common.domain;

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