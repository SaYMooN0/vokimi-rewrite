using MediatR;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Application.Tests.formats_shared.events;

internal class TestEditorsListChangedEventHandler : INotificationHandler<TestEditorsListChangedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public TestEditorsListChangedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(TestEditorsListChangedEvent notification, CancellationToken cancellationToken) {
        var editorsToAdd = notification.newEditors.Except(notification.oldEditors);
        var editorsToRemove = notification.oldEditors.Except(notification.newEditors);
        List<AppUser> updateList = new List<AppUser>(editorsToAdd.Count() + editorsToRemove.Count());
        
        foreach (var editorIdToAdd in editorsToAdd) {
            var userToAdd = await _appUsersRepository.GetById(editorIdToAdd);
            if (userToAdd is not null) {
                userToAdd.AddEditorRoleForTest(notification.TestId);
                updateList.Add(userToAdd);
            }
        }

        foreach (var editorIdToRemove in editorsToRemove) {
            var userToRemove = await _appUsersRepository.GetById(editorIdToRemove);
            if (userToRemove is not null) {
                userToRemove.RemoveEditorRoleForTest(notification.TestId);
                updateList.Add(userToRemove);
            }
        }
        await _appUsersRepository.UpdateRange(updateList);
    }
}
