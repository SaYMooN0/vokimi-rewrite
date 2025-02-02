using SharedKernel.Common.domain.interfaces;

namespace SharedKernel.Common.domain;
public abstract class Entity<IdType> where IdType : IEntityId
{
    public IdType Id { get; init; }

    public override bool Equals(object? other) {
        if (other is null || other.GetType() != GetType()) {
            return false;
        }
        Entity<IdType> otherEntity = (Entity<IdType>)other;
        return Id.Equals(otherEntity.Id);
    }
    public override int GetHashCode() =>
        Id.GetHashCode();

}