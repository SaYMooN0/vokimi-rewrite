
namespace SharedKernel.Common.EntityIds;

public abstract class EntityId : ValueObject
{
    public Guid Value { get; init; }
    protected EntityId(Guid value) => Value = value;
    public override string ToString() => Value.ToString();
    public override IEnumerable<object> GetEqualityComponents() { yield return Value; }

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
