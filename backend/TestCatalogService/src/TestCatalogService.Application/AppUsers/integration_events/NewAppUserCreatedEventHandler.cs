using MediatR;
using SharedKernel.IntegrationEvents.authentication;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.AppUserAggregate;

namespace TestCatalogService.Application.AppUsers.integration_events;

internal class NewAppUserCreatedEventHandler : INotificationHandler<NewAppUserCreatedIntegrationEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public NewAppUserCreatedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(NewAppUserCreatedIntegrationEvent notification, CancellationToken cancellationToken) {
        var newUser = new AppUser(notification.CreatedUserId);
        await _appUsersRepository.Add(newUser);
    }
}