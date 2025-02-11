using MediatR;
using SharedKernel.Common.errors;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Domain.AppUserAggregate;
using TestTakingService.Domain.AppUserAggregate.events;

namespace TestTakingService.Application.AppUsers.events;

public class
    TestFeedbackLeftByAuthenticatedUserEventHandler : INotificationHandler<TestFeedbackLeftByAuthenticatedUserEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public TestFeedbackLeftByAuthenticatedUserEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(TestFeedbackLeftByAuthenticatedUserEvent notification,
        CancellationToken cancellationToken
    ) {
        AppUser? user = await _appUsersRepository.GetById(notification.AppUserId);
        if (user is null) {
            throw new ErrCausedException(Err.ErrPresets.UserNotFound(notification.AppUserId));
        }

        user.AddFeedbackRecordId(notification.FeedbackRecordId);
        await _appUsersRepository.Update(user);
    }
}