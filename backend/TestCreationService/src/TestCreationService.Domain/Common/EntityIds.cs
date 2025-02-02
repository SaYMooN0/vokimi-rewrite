using SharedKernel.Common.domain;

namespace TestCreationService.Domain.Common;
public class TestInteractionsAccessSettingsId : EntityId
{
    public TestInteractionsAccessSettingsId(Guid value) : base(value) { }
    public static TestInteractionsAccessSettingsId CreateNew() => new(Guid.CreateVersion7());
}
public class GeneralTestTakingProcessSettingsId : EntityId
{
    public GeneralTestTakingProcessSettingsId(Guid value) : base(value) { }
    public static GeneralTestTakingProcessSettingsId CreateNew() => new(Guid.CreateVersion7());
}
public class TestTagsListId : EntityId
{
    public TestTagsListId(Guid value) : base(value) { }
    public static TestTagsListId CreateNew() => new(Guid.CreateVersion7());
}

