using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestManagingService.Domain.TestAggregate.formats_shared;
using TestManagingService.Domain.TestAggregate.tier_list_format.events;

namespace TestManagingService.Domain.TestAggregate.tier_list_format;

public class TierListFormatTest : BaseTest
{
    private TierListFormatTest() { }
    public override TestFormat Format => TestFormat.TierList;
    public TierListTestFeedbackOption FeedbackOption { get; private set; }

    public TierListFormatTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate,
        TestInteractionsAccessSettings interactionsAccessSettings,
        TierListTestFeedbackOption feedbackOption
    ) : base(testId, creatorId, editorIds, publicationDate, interactionsAccessSettings) {
        FeedbackOption = feedbackOption;
    }

    public ErrOrNothing SetFeedbackOptionEnabled(
        AnonymityValues anonymity,
        string accompanyingText,
        ushort maxFeedbackLength
    ) {
        var creationRes = TierListTestFeedbackOption.Enabled.CreateNew(
            anonymity, accompanyingText, maxFeedbackLength
        );
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        FeedbackOption = creationRes.GetSuccess();
        _domainEvents.Add(new TierListTestFeedbackOptionUpdatedEvent(
            Id, FeedbackOption
        ));
        return ErrOrNothing.Nothing;
    }

    public void SetFeedbackOptionDisabled() {
        FeedbackOption = TierListTestFeedbackOption.Disabled.Instance;
        _domainEvents.Add(new TierListTestFeedbackOptionUpdatedEvent(Id, FeedbackOption));
    }
}