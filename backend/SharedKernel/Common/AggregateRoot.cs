namespace SharedKernel.Common;

public abstract class AggregateRoot : Entity
{
    protected readonly List<IDomainEvent> _domainEvents = new();

    public List<IDomainEvent> PopDomainEvents() {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }
}