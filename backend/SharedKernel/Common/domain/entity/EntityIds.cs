using SharedKernel.Common.domain.value_object;
using SharedKernel.Common.errors;

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
public class TestCommentId : EntityId
{
    public TestCommentId(Guid value) : base(value) { }

    public static TestCommentId CreateNew() => new(Guid.CreateVersion7());
}

public class TestTagId : IEntityId
{
    public string Value { get; init; }

    public TestTagId(string value) {
        if (!TestTagsRules.IsStringValidTag(value)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData($"'{value}' is not a valid tag"));
        }
        Value = value;
    }
    public static ErrOr<TestTagId> Create(string value) {
        if (!TestTagsRules.IsStringValidTag(value)) {
            return Err.ErrFactory.InvalidData($"'{value}' is not a valid tag");
        }

        return new TestTagId(value);
    }

    public override string ToString() => Value;
}
