using MediatR;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;

namespace TestCatalogService.Application.Tests.formats_shared.events;

internal class TestCommentToTestAddedEventHandler : INotificationHandler<TestCommentToTestAddedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    public TestCommentToTestAddedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }


    public async Task Handle(TestCommentToTestAddedEvent notification, CancellationToken cancellationToken) {
        AppUser? appUser = await _appUsersRepository.GetById(notification.AuthorId);
        if (appUser is null) {
            throw new ErrCausedException(Err.ErrFactory.NotFound(
                "Unknown user. User that is trying to leave a comment was not found",
                $"User id: {notification.AuthorId}"
            ));
        }

        appUser.AddComment(notification.CommentId);
        await _appUsersRepository.Update(appUser);
    }
}