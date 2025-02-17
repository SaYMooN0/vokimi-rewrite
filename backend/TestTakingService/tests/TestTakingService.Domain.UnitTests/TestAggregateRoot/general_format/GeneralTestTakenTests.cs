using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.general_format;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestTakenTests
{
    private readonly AppUserId _creatorId = new AppUserId(Guid.NewGuid());
    private readonly TestId _testId = new TestId(Guid.NewGuid());

    private static readonly GeneralTestQuestion _question1 = new();
    private static readonly GeneralTestQuestion _question2 = new();
    private static readonly GeneralTestQuestion _question3 = new();
    private static readonly GeneralTestQuestion _question4 = new();
    private static readonly GeneralTestQuestion _question5 = new();

    private static readonly GeneralTestResult _result1 = new();
    private static readonly GeneralTestResult _result2 = new();
    private static readonly GeneralTestResult _result3 = new();
    private static readonly GeneralTestResult _result4 = new();
    private static readonly GeneralTestResult _result5 = new();

    private readonly ImmutableArray<GeneralTestQuestion> _questions = [
        _question1, _question2, _question3, _question4, _question5
    ];

    private readonly ImmutableArray<GeneralTestResult> _results = [
        _result1, _result2, _result3, _result4, _result5
    ];

    private GeneralFormatTest CreateTest(
        AccessLevel testAccessLevel = AccessLevel.Public,
        IReadOnlyCollection<GeneralTestQuestion> questions = null,
        IReadOnlyCollection<GeneralTestResult> results = null,
        GeneralTestFeedbackOption feedbackOption = null,
        bool shuffleQuestions = false
    ) => new GeneralFormatTest(
        _testId,
        _creatorId,
        [],
        testAccessLevel,
        TestStylesSheet.CreateNew(_testId),
        questions ?? _questions,
        shuffleQuestions,
        results ?? _results,
        feedbackOption ?? GeneralTestFeedbackOption.Disabled.Instance
    );
}