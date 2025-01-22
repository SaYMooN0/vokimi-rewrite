using SharedKernel.Common.EntityIds;

namespace SharedKernel.Common;
public abstract class Entity<IdType> where IdType : EntityId
{
    public IdType Id { get; init; }

    public override bool Equals(object? other) {
        if (other is null || other.GetType() != GetType()) {
            return false;
        }

        return Id == ((Entity<IdType>)other).Id;
    }


    public override int GetHashCode() =>
        Id.GetHashCode();

}