using MediatR;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Application.Tests.formats_shared.events;

internal class NewTestInitializedEventHandler : INotificationHandler<NewTestInitializedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public NewTestInitializedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(NewTestInitializedEvent notification, CancellationToken cancellationToken) {
        AppUser? appUser = await _appUsersRepository.GetById(notification.CreatorId);
        if (appUser is null) {
            throw new ErrCausedException(Err.ErrFactory.NotFound("Unable to find user that is trying to create test"));
        }
        appUser.AddCreatedTest(notification.TestId);
        await _appUsersRepository.Update(appUser);
    }
}
