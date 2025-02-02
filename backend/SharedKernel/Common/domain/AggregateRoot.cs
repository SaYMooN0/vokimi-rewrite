using SharedKernel.Common.domain.interfaces;

namespace SharedKernel.Common.domain;

public abstract class AggregateRoot<IdType> : Entity<IdType>, IAggregateRoot where IdType : IEntityId
{
    protected readonly List<IDomainEvent> _domainEvents = new();

    public List<IDomainEvent> PopDomainEvents() {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }
}