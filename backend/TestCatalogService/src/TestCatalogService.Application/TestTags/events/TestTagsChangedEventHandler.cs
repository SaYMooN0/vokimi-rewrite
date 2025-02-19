using MediatR;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestTagAggregate;
using TestCatalogService.Domain.TestTagAggregate.events;
namespace TestCatalogService.Application.TestTags.events;

internal class TestTagsChangedEventHandler : INotificationHandler<TestTagsChangedEvent>
{
    private ITestTagsRepository _testTagsRepository;

    public TestTagsChangedEventHandler(ITestTagsRepository testTagsRepository) {
        _testTagsRepository = testTagsRepository;
    }

    public async Task Handle(TestTagsChangedEvent notification, CancellationToken cancellationToken) {
        var addedTags = notification.NewTags.Except(notification.OldTags);
        var removedTags = notification.OldTags.Except(notification.NewTags);

        foreach (var tagId in addedTags) {
            var tag = await _testTagsRepository.GetById(tagId);
            if (tag is null) {
                TestTag tagToAdd = TestTag.Create(tagId);
                tagToAdd.IncrementTestWithThisTagCount();
                await _testTagsRepository.AddNew(tagToAdd);
            } else {
                tag.IncrementTestWithThisTagCount();
                await _testTagsRepository.Update(tag);
            }
        }

        foreach (var tagId in removedTags) {
            var tag = await _testTagsRepository.GetById(tagId);
            if (tag is not null) {
                tag.DecrementTestWithThisTagCount();
                await _testTagsRepository.Update(tag);
            }
        }
    }
}
