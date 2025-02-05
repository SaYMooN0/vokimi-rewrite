using MediatR;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate.general_format.events;

namespace TestCreationService.Application.Tests.general_format.events;

public class GeneralFormatTestDeletedHandler { }

internal class GeneralFormatTestDeletedEventHandler : INotificationHandler<GeneralFormatTestDeletedEvent>
{
    private IGeneralFormatTestsRepository _generalFormatTestsRepository;
    private IAppUsersRepository _appUsersRepository;

    public GeneralFormatTestDeletedEventHandler(
        IGeneralFormatTestsRepository generalFormatTestsRepository,
        IAppUsersRepository appUsersRepository
    ) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(GeneralFormatTestDeletedEvent notification, CancellationToken cancellationToken) {
        await _generalFormatTestsRepository.Delete(notification.TestId);
        AppUser? creator = await _appUsersRepository.GetById(notification.CreatorId);
        if (creator is not null) {
            creator.RemoveCreatedTest(notification.TestId);
            await _appUsersRepository.Update(creator);
        }

        foreach (var editorId in notification.EditorIds) {
            AppUser? editor = await _appUsersRepository.GetById(editorId);
            if (editor is not null) {
                editor.RemoveEditorRoleForTest(notification.TestId);
                await _appUsersRepository.Update(editor);
            }
        }
    }
}