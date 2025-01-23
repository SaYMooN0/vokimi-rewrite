namespace SharedKernel.Common.domain;

public interface IAggregateRoot
{
    public List<IDomainEvent> PopDomainEvents();
}
