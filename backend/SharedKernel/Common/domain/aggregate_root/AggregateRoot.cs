using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;

namespace SharedKernel.Common.domain.aggregate_root;

public abstract class AggregateRoot<IdType> : Entity<IdType>, IAggregateRoot where IdType : IEntityId
{
    protected readonly List<IDomainEvent> _domainEvents = new();

    public IImmutableList<IDomainEvent> GetDomainEventsCopy() => _domainEvents.ToImmutableList();

    public List<IDomainEvent> PopDomainEvents() {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }
}