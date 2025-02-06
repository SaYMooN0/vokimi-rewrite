using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.general_format;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;
    protected virtual ImmutableArray<GeneralTestQuestion> _questions { get; init; }
    private bool _shuffleQuestions { get; init; }
    protected virtual ImmutableArray<GeneralTestResult> _results { get; init; }
    private GeneralTestFeedbackOption _feedbackOption { get; init; }

    public GeneralFormatTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<GeneralTestQuestion> questions,
        bool shuffleQuestions,
        ImmutableArray<GeneralTestResult> results,
        GeneralTestFeedbackOption feedbackOption
    ) {
        Id = testId;
        _creatorId = creatorId;
        _questions = questions;
        _shuffleQuestions = shuffleQuestions;
        _results = results;
        _feedbackOption = feedbackOption;
    }
}