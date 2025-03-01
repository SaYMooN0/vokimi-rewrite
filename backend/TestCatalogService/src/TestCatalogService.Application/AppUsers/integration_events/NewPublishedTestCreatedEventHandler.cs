using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;

namespace TestCatalogService.Application.AppUsers.integration_events;

internal class NewPublishedTestCreatedEventHandler : INotificationHandler<NewPublishedTestCreatedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public NewPublishedTestCreatedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(NewPublishedTestCreatedEvent notification, CancellationToken cancellationToken) {
        AppUser? creator = await _appUsersRepository.GetById(notification.CreatorId);
        if (creator is null) {
            creator = new AppUser(notification.CreatorId);
            await _appUsersRepository.Add(creator);
        }

        creator.AddCreatedTest(notification.TestId);
        await _appUsersRepository.Update(creator);
        foreach (AppUserId editorId in notification.EditorIds) {
            AppUser? editor = await _appUsersRepository.GetById(editorId);
            if (editor is null) {
                editor = new AppUser(editorId);
                await _appUsersRepository.Add(editor);
            }

            editor.AddEditorRoleForTest(notification.TestId);
            await _appUsersRepository.Update(editor);
        }
    }
}