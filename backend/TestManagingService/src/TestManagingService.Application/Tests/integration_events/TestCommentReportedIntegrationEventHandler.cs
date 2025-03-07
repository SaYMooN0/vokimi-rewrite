using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.IntegrationEvents.test_managing;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Application.Tests.integration_events;

public class TestCommentReportedIntegrationEventHandler : INotificationHandler<TestCommentReportedIntegrationEvent>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TestCommentReportedIntegrationEventHandler(
        IBaseTestsRepository baseTestsRepository, IDateTimeProvider dateTimeProvider
    ) {
        _baseTestsRepository = baseTestsRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(TestCommentReportedIntegrationEvent notification, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetWithCommentReports(notification.TestId);
        if (test is null) {
            throw new ErrCausedException(Err.ErrPresets.TestNotFound(notification.TestId));
        }

        test.AddCommentReport(
            notification.ReportAuthorId,
            notification.CommentId,
            notification.Text,
            notification.Reason,
            _dateTimeProvider
        );

        await _baseTestsRepository.Update(test);
    }
}