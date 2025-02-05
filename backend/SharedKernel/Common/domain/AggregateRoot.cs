using SharedKernel.Common.domain.interfaces;
using System.Collections.Immutable;

namespace SharedKernel.Common.domain;

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