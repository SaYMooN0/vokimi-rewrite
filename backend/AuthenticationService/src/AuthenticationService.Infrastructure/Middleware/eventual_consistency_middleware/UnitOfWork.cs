using MediatR;
using System.Data;
using SharedKernel.Common.domain.aggregate_root;

namespace AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;

public class UnitOfWork : IDisposable
{
    private readonly List<IAggregateRoot> _trackedAggregates = new();
    private IDbTransaction? _transaction;
    private IDbConnection? _connection;
    private bool _isCommitted = false;

    public IDbConnection Connection => _connection ?? throw new InvalidOperationException("Connection not initialized.");
    public IDbTransaction Transaction => _transaction ?? throw new InvalidOperationException("Transaction not initialized.");

    public void BeginTransaction(IDbConnection connection) {
        if (_connection != null)
            throw new InvalidOperationException("Transaction already started.");

        _connection = connection;
        _transaction = _connection.BeginTransaction();
        _isCommitted = false;
    }

    public void TrackAggregate(IAggregateRoot aggregate) {
        if (!_trackedAggregates.Contains(aggregate)) {
            _trackedAggregates.Add(aggregate);
        }
    }

    public async Task PublishDomainEventsAsync(IPublisher publisher) {
        var domainEvents = _trackedAggregates
            .SelectMany(aggregate => aggregate.PopDomainEvents())
            .ToList();
        if (domainEvents.Count == 0) {
            return;
        }
        foreach (var domainEvent in domainEvents) {
            await publisher.Publish(domainEvent);
        }
        await PublishDomainEventsAsync(publisher);
    }

    public void Commit() {
        if (_transaction == null || _isCommitted)
            throw new InvalidOperationException("Transaction already committed or not started.");

        _transaction.Commit();
        _isCommitted = true;
    }

    public void Rollback() {
        if (_transaction != null) {
            _transaction.Rollback();
        }
    }

    public void Dispose() {
        _transaction?.Dispose();
        _transaction = null;
        _connection = null;
    }
}