using SharedKernel.Common.EntityIds;

namespace SharedKernel.Common;
public abstract class Entity
{
    protected abstract EntityId EntityId { get; }

    public override bool Equals(object? other) {
        if (other is null || other.GetType() != GetType()) {
            return false;
        }

        return ((Entity)other).EntityId.Value == EntityId.Value;
    }

    public override int GetHashCode() {
        return EntityId.Value.GetHashCode();
    }

}