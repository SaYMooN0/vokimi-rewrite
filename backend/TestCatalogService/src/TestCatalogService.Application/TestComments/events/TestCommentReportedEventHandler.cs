using MediatR;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Application.TestComments.events;

public class TestCommentReportedEventHandler : INotificationHandler<TestCommentReportedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly ITestCommentsRepository _testCommentsRepository;

    public TestCommentReportedEventHandler(
        IAppUsersRepository appUsersRepository, ITestCommentsRepository testCommentsRepository
    ) {
        _appUsersRepository = appUsersRepository;
        _testCommentsRepository = testCommentsRepository;
    }

    public async Task Handle(TestCommentReportedEvent notification, CancellationToken cancellationToken) {
        TestComment? comment = await _testCommentsRepository.GetById(notification.CommentId);
        if (comment is null) {
            throw new ErrCausedException(Err.ErrFactory.NotFound(
                "Reported comment was not found",
                $"Comment id: {notification.CommentId}"
            ));
        }

        if (comment.IsDeleted) {
            throw new ErrCausedException(new Err(
                "Deleted comment cannot be reported",
                details:
                $"Comment id: {comment.Id}. Deleted at: {comment.DeletedAt?.ToShortDateString() ?? "(Not set)"}"
            ));
        }

        AppUser
            ? reportAuthor = await _appUsersRepository.GetById(notification.ReportAuthorId);
        if (reportAuthor is null) {
            throw new ErrCausedException(Err.ErrFactory.NotFound(
                "User that created the comment report was not found",
                $"User id: {notification.ReportAuthorId}"
            ));
        }

        reportAuthor.AddCommentReport(notification.ReportId);
        await _appUsersRepository.Update(reportAuthor);
    }
}