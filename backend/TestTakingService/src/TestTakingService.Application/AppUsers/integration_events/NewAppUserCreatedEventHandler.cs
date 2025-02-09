using MediatR;
using SharedKernel.IntegrationEvents.authentication;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Domain.AppUserAggregate;

namespace TestTakingService.Application.AppUsers.integration_events;

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