using System.Collections.Immutable;

namespace SharedKernel.Common.domain.interfaces;

public interface IAggregateRoot
{
    public List<IDomainEvent> PopDomainEvents();
    public IImmutableList<IDomainEvent> GetDomainEventsCopy();
}
