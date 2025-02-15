using MediatR;
using TestCatalogService.Domain.TestCommentAggregate.events;

namespace TestCatalogService.Application.TestComments.events;

internal class NewTestCommentCreatedEventHandler : INotificationHandler<NewTestCommentCreatedEvent>
{
    public Task Handle(NewTestCommentCreatedEvent notification, CancellationToken cancellationToken) {
        ...
    }
}