
namespace SharedKernel.Common.EntityIds;

public abstract class EntityId
{
    public Guid Value { get; init; }
    protected EntityId(Guid value) => Value = value;
    public override string ToString() => Value.ToString();
}

public class AppUserId : EntityId
{
    public AppUserId(Guid value) : base(value) { }

    public static AppUserId CreateNew() => new(Guid.CreateVersion7());
}
public class TestId : EntityId
{
    public TestId(Guid value) : base(value) { }

    public static TestId CreateNew() => new(Guid.CreateVersion7());
}
