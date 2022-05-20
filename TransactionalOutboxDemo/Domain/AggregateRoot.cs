namespace TransactionalOutboxDemo.Domain;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void RegisterDomainEvent(IDomainEvent e) => _domainEvents.Add(e);
    public void ClearEvents() => _domainEvents.Clear();
}
