using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

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

public class TierListTestTakenRecordDetailsId : EntityId
{
    public TierListTestTakenRecordDetailsId(Guid value) : base(value) { }
    public static TierListTestTakenRecordDetailsId CreateNew() => new(Guid.CreateVersion7());
}