using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.IntegrationEvents.test_managing.tags;
using SharedKernel.IntegrationEvents.test_publishing;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Application.Tests.integration_events;

internal class PublishedTestTagsChangedIntegrationEventHandler :
    INotificationHandler<PublishedTestTagsChangedIntegrationEvent>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public PublishedTestTagsChangedIntegrationEventHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task Handle(
        PublishedTestTagsChangedIntegrationEvent notification,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetById(notification.TestId);
        if (test is null) {
            throw new ErrCausedException(Err.ErrPresets.TestNotFound(notification.TestId));
        }

        HashSet<TestTagId> newTags = notification.NewTags.Select(t => new TestTagId(t)).ToHashSet();
        test.UpdateTags(newTags);
        await _baseTestsRepository.Update(test);
    }
}