using System.Collections.Immutable;

namespace SharedKernel.Common.domain.aggregate_root;

public interface IAggregateRoot
{
    public List<IDomainEvent> PopDomainEvents();
    public IImmutableList<IDomainEvent> GetDomainEventsCopy();
}
