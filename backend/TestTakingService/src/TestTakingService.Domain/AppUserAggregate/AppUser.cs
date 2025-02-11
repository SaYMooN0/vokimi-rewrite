using System.Collections.Immutable;
using SharedKernel.Common.domain;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private readonly HashSet<TestId> _takenTestIds;
    private readonly HashSet<TestTakenRecordId> _testTakenRecordIds;
    private readonly HashSet<TestFeedbackRecordId> _feedbackRecordIds;
    private AppUser() { }

    public AppUser(AppUserId id) {
        Id = id;
        _takenTestIds = [];
        _testTakenRecordIds = [];
        _feedbackRecordIds = [];
    }

    public ImmutableHashSet<TestId> TakenTestIds => _takenTestIds.ToImmutableHashSet();
    public ImmutableHashSet<TestTakenRecordId> TestTakenRecordIds => _testTakenRecordIds.ToImmutableHashSet();
    public ImmutableHashSet<TestFeedbackRecordId> FeedbackRecordIds => _feedbackRecordIds.ToImmutableHashSet();

    public void AddTakenTestId(TestId testId) {
        _takenTestIds.Add(testId);
    }

    public void AddTestTakenRecordId(TestTakenRecordId testTakenRecordId) {
        _testTakenRecordIds.Add(testTakenRecordId);
    }

    public void AddFeedbackRecordId(TestFeedbackRecordId feedbackRecordId) {
        _feedbackRecordIds.Add(feedbackRecordId);
    }
}