using MediatR;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;

namespace TestCatalogService.Application.AppUsers.events;

internal class NewPublishedTestCreatedEventHandler : INotificationHandler<NewPublishedTestCreatedEvent>
{
}
