using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.general_format;
using TestManagingService.Domain.TestAggregate.general_format.events;

namespace TestManagingService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;
    public GeneralTestFeedbackOption FeedbackOption { get; private set; }

    private GeneralFormatTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate
    ) : base(testId, creatorId, editorIds, publicationDate) { }

    public static GeneralFormatTest CreateNew(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate
    ) => new(testId, creatorId, editorIds, publicationDate);

    public ErrOrNothing SetFeedbackOptionEnabled(
        AnonymityValues anonymity,
        string accompanyingText,
        ushort maxFeedbackLength
    ) {
        var creationRes = GeneralTestFeedbackOption.Enabled.CreateNew(
            anonymity, accompanyingText, maxFeedbackLength
        );
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        FeedbackOption = creationRes.GetSuccess();
        _domainEvents.Add(new GeneralTestFeedbackOptionUpdatedEvent(
            Id, FeedbackOption
        ));
        return ErrOrNothing.Nothing;
    }

    public void SetFeedbackOptionDisabled() {
        FeedbackOption = GeneralTestFeedbackOption.Disabled.Instance;
        _domainEvents.Add(new GeneralTestFeedbackOptionUpdatedEvent(
            Id, FeedbackOption
        ));
    }
}