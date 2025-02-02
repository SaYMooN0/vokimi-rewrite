namespace SharedKernel.Common.domain.interfaces;

public interface IAggregateRoot
{
    public List<IDomainEvent> PopDomainEvents();
}
