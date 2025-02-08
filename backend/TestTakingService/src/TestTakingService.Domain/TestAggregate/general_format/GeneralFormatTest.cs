using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.general_format;
using SharedKernel.Common.tests.test_styles;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;
    public virtual ImmutableArray<GeneralTestQuestion> Questions { get; init; }
    private bool _shuffleQuestions { get; init; }
    protected virtual ImmutableArray<GeneralTestResult> _results { get; init; }
    public GeneralTestFeedbackOption FeedbackOption { get; init; }

    public GeneralFormatTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableHashSet<AppUserId> editors,
        AccessLevel accessLevel,
        TestStylesSheet styles,
        //general format specific
        ImmutableArray<GeneralTestQuestion> questions,
        bool shuffleQuestions,
        ImmutableArray<GeneralTestResult> results,
        GeneralTestFeedbackOption feedbackOption
    ) : base(testId, creatorId, editors, accessLevel, styles) {
        Questions = questions;
        _shuffleQuestions = shuffleQuestions;
        _results = results;
        FeedbackOption = feedbackOption;
    }
}