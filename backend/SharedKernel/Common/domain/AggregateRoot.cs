using SharedKernel.Common.domain;
using SharedKernel.Common.EntityIds;

namespace SharedKernel.Common;

public abstract class AggregateRoot<IdType> : Entity<IdType>, IAggregateRoot where IdType : EntityId
{
    protected readonly List<IDomainEvent> _domainEvents = new();

    public List<IDomainEvent> PopDomainEvents() {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }
}