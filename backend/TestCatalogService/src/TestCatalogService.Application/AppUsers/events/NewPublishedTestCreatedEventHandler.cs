using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;

namespace TestCatalogService.Application.AppUsers.events;

internal class NewPublishedTestCreatedEventHandler : INotificationHandler<NewPublishedTestCreatedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public NewPublishedTestCreatedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(NewPublishedTestCreatedEvent notification, CancellationToken cancellationToken) {
        AppUser? creator = await _appUsersRepository.GetById(notification.CreatorId);
        if (creator is null) {
            throw new ErrCausedException(
                Err.ErrFactory.NotFound("User that is meant to be the creator was not found")
            );
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