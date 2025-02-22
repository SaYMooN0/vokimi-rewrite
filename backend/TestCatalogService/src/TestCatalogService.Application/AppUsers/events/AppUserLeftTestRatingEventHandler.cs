using MediatR;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.AppUserAggregate;
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
        AppUser? user = await _appUsersRepository.GetById(notification.UserId);
        if (user is null) {
            throw new ErrCausedException(Err.ErrFactory.NotFound(
                "Unable to update rating because user was not found",
                $"User id: {notification.UserId}"
            ));
        }
        user.AddRating(notification.RatingId);
        await _appUsersRepository.Update(user);
    }
}