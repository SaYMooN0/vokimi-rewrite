using MediatR;
using TestCatalogService.Domain.AppUserAggregate.events;
using TestCatalogService.Domain.Common.interfaces.repositories;

namespace TestCatalogService.Application.AppUsers.events;

public class AppUserLeftTestRatingEventHandler : INotificationHandler<AppUserLeftTestRatingEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    public AppUserLeftTestRatingEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(AppUserLeftTestRatingEvent notification, CancellationToken cancellationToken) {
        _appUsersRepository
    }
}