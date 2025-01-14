using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.AppUserAggregate;
using AuthenticationService.Domain.AppUserAggregate.events;
using MediatR;
using SharedKernel.Common;

namespace AuthenticationService.Application.AppUsers.events;

internal class UserConfirmedEventHandler : INotificationHandler<UserConfirmedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUnconfirmedAppUsersRepository _unconfirmedAppUsersRepository;

    public UserConfirmedEventHandler(IAppUsersRepository appUsersRepository, IUnconfirmedAppUsersRepository unconfirmedAppUsersRepository) {
        _appUsersRepository = appUsersRepository;
        _unconfirmedAppUsersRepository = unconfirmedAppUsersRepository;
    }

    public async Task Handle(UserConfirmedEvent notification, CancellationToken cancellationToken) {
        var appUser = AppUser.CreateNew(notification.Email, notification.PasswordHash, UtcDateTimeProvider.Instance);
        await _unconfirmedAppUsersRepository.RemoveById(notification.UnconfirmedAppUserId);
        await _appUsersRepository.Add(appUser);
    }
}
