using MediatR;
using SharedKernel.Common.errors;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Application.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestCommentAggregate.events;

namespace TestCatalogService.Application.TestComments.events;

internal class NewTestCommentCreatedEventHandler : INotificationHandler<NewTestCommentCreatedEvent>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IAppUsersRepository _appUsersRepository;

    public NewTestCommentCreatedEventHandler(
        IAppUsersRepository appUsersRepository,
        IBaseTestsRepository baseTestsRepository
    ) {
        _appUsersRepository = appUsersRepository;
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task Handle(NewTestCommentCreatedEvent notification, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(notification.TestId);
        if (test is null) {
            throw new ErrCausedException(Err.ErrFactory.NotFound(
                "Unknown test. Test for which the comment must be created was not found",
                $"TestId: {notification.TestId}"
            ));
        }

        test.AddComment(notification.CommentId);
        await _baseTestsRepository.Update(test);

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