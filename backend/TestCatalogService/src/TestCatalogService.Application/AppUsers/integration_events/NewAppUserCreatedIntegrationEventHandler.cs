using MediatR;
using SharedKernel.IntegrationEvents.authentication;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.Common.interfaces.repositories;

namespace TestCatalogService.Application.AppUsers.integration_events;

internal class NewAppUserCreatedIntegrationEventHandler : INotificationHandler<NewAppUserCreatedIntegrationEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public NewAppUserCreatedIntegrationEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(NewAppUserCreatedIntegrationEvent notification, CancellationToken cancellationToken) {
        var newUser = new AppUser(notification.CreatedUserId);
        await _appUsersRepository.Add(newUser);
    }
}