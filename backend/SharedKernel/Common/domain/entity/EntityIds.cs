using SharedKernel.Common.domain.value_object;

namespace SharedKernel.Common.domain.entity;

public abstract class EntityId : ValueObject, IComparable, IEntityId
{
    public Guid Value { get; init; }
    protected EntityId(Guid value) => Value = value;
    public override string ToString() => Value.ToString();

    public override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }

    public int CompareTo(object? obj) => obj switch {
        IEntityId ed => ToString().CompareTo(ed.ToString()),
        Guid guid => guid.CompareTo(Value),
        _ => -1
    };
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

public class GeneralTestQuestionId : EntityId
{
    public GeneralTestQuestionId(Guid value) : base(value) { }

    public static GeneralTestQuestionId CreateNew() => new(Guid.CreateVersion7());
}

public class GeneralTestAnswerId : EntityId
{
    public GeneralTestAnswerId(Guid value) : base(value) { }

    public static GeneralTestAnswerId CreateNew() => new(Guid.CreateVersion7());
}

public class GeneralTestResultId : EntityId
{
    public GeneralTestResultId(Guid value) : base(value) { }

    public static GeneralTestResultId CreateNew() => new(Guid.CreateVersion7());
}

public class TestStylesSheetId : EntityId
{
    public TestStylesSheetId(Guid value) : base(value) { }

    public static TestStylesSheetId CreateNew() => new(Guid.CreateVersion7());
}

