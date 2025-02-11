using MediatR;
using SharedKernel.Common.errors;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Domain.AppUserAggregate;
using TestTakingService.Domain.AppUserAggregate.events;

namespace TestTakingService.Application.AppUsers.events;

public class TestTakenByAuthenticatedUserEventHandler : INotificationHandler<TestTakenByAuthenticatedUserEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public TestTakenByAuthenticatedUserEventHandler(
        IAppUsersRepository appUsersRepository
    ) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(TestTakenByAuthenticatedUserEvent notification, CancellationToken cancellationToken) {
        AppUser? user = await _appUsersRepository.GetById(notification.AppUserId);
        if (user is null) {
            throw new ErrCausedException(Err.ErrPresets.UserNotFound(notification.AppUserId));
        }

        user.AddTakenTestId(notification.TestId);
        user.AddTestTakenRecordId(notification.TestTakenRecordId);
        await _appUsersRepository.Update(user);
    }
}