using AuthenticationService.Application.Common.interfaces.repositories;
using AuthenticationService.Domain.AppUserAggregate;
using AuthenticationService.Domain.AppUserAggregate.events;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Common.interfaces;

namespace AuthenticationService.Application.AppUsers.events;

internal class UserConfirmedEventHandler : INotificationHandler<UserConfirmedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUnconfirmedAppUsersRepository _unconfirmedAppUsersRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UserConfirmedEventHandler(IAppUsersRepository appUsersRepository, IUnconfirmedAppUsersRepository unconfirmedAppUsersRepository, IDateTimeProvider dateTimeProvider) {
        _appUsersRepository = appUsersRepository;
        _unconfirmedAppUsersRepository = unconfirmedAppUsersRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(UserConfirmedEvent notification, CancellationToken cancellationToken) {
        var appUser = AppUser.CreateNew(notification.Email, notification.PasswordHash, _dateTimeProvider);
        await _unconfirmedAppUsersRepository.RemoveById(notification.UnconfirmedAppUserId);
        await _appUsersRepository.Add(appUser);
    }
}
