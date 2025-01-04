
namespace SharedKernel.Common.EntityIds;

public abstract class EntityId
{
    public Guid Value { get; init; }
}

public class AppUserId : EntityId
{
    private AppUserId(Guid value) { Value = value; }
    public static AppUserId CreateNew() => new(Guid.CreateVersion7());
}
