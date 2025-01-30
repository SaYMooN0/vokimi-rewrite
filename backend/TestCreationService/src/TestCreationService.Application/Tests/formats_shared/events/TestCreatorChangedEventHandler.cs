using MediatR;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Application.Tests.formats_shared.events;

internal class TestCreatorChangedEventHandler : INotificationHandler<TestCreatorChangedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public TestCreatorChangedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(TestCreatorChangedEvent notification, CancellationToken cancellationToken) {
        AppUser? newCreator = await _appUsersRepository.GetById(notification.NewCreator);
        if (newCreator is null) {
            throw new ErrCausedException(new Err("Cannot add test to the user's created tests list because the user does not exist"));
        }
        newCreator.AddCreatedTest(notification.TestId);
        await _appUsersRepository.Update(newCreator);
        AppUser? oldCreator = await _appUsersRepository.GetById(notification.OldCreator);
        if (oldCreator is not null) {
            oldCreator.RemoveCreatedTest(notification.TestId);
            await _appUsersRepository.Update(oldCreator);
        }
    }
}