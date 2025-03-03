using MediatR;

namespace DBSeeder;

internal class FakePublisher : IPublisher
{
    public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken()) {
        return Task.CompletedTask;
    }

    public Task Publish<TNotification>(
        TNotification notification, CancellationToken
            cancellationToken = new CancellationToken()
    ) where TNotification : INotification {
        return Task.CompletedTask;
    }
}