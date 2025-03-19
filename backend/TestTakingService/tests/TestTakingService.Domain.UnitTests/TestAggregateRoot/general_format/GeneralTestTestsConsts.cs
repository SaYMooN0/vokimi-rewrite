using System.Collections.Immutable;
using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.general_format;
using SharedKernel.Common.tests.value_objects;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot.general_format;

public static class GeneralTestTestsConsts
{
    public static GeneralTestResultId Result1Id = new(Guid.NewGuid());
    public static GeneralTestResultId Result2Id = new(Guid.NewGuid());
    public static GeneralTestResultId Result3Id = new(Guid.NewGuid());
    public static GeneralTestResultId Result4Id = new(Guid.NewGuid());
    public static GeneralTestResultId Result5Id = new(Guid.NewGuid());

    public static readonly GeneralTestResult Result1 = new(
        Result1Id, "Explorer", "You are an explorer, always seeking new knowledge!", "explorer.png"
    );

    public static readonly GeneralTestResult Result2 = new(
        Result2Id, "Scientist", "You have a scientific mind, always analyzing and discovering!", "scientist.png"
    );

    public static readonly GeneralTestResult Result3 = new(
        Result3Id, "Philosopher", "You think deeply about the world and seek wisdom.", "philosopher.png"
    );

    public static readonly GeneralTestResult Result4 = new(
        Result4Id,
        name: "Artist", "You have a creative mind, always seeing beauty in the world.", "artist.png"
    );

    public static readonly GeneralTestResult Result5 = new(
        Result5Id,
        "Mathematician", "You have a logical mind and are always solving complex problems.", "mathematician.png"
    );

    public readonly static GeneralTestAnswerId Q1Answer1Id = GeneralTestAnswerId.CreateNew();
    public readonly static GeneralTestAnswerId Q1Answer2Id = GeneralTestAnswerId.CreateNew();
    public readonly static GeneralTestAnswerId Q2Answer1Id = GeneralTestAnswerId.CreateNew();
    public readonly static GeneralTestAnswerId Q2Answer2Id = GeneralTestAnswerId.CreateNew();
    public readonly static GeneralTestAnswerId Q3Answer1Id = GeneralTestAnswerId.CreateNew();
    public readonly static GeneralTestAnswerId Q3Answer2Id = GeneralTestAnswerId.CreateNew();
    public readonly static GeneralTestAnswerId Q4Answer1Id = GeneralTestAnswerId.CreateNew();
    public readonly static GeneralTestAnswerId Q4Answer2Id = GeneralTestAnswerId.CreateNew();

    public static readonly GeneralTestAnswer Q1Answer1 = new GeneralTestAnswer(
        Q1Answer1Id,
        GeneralTestAnswerTypeSpecificData.ImageAndText.CreateNew("Paris", "paris_image.png").GetSuccess(),
        [Result1]
    );

    public static readonly GeneralTestAnswer Q1Answer2 = new GeneralTestAnswer(
        Q1Answer2Id,
        GeneralTestAnswerTypeSpecificData.ImageAndText.CreateNew("London", "london_image.png").GetSuccess(),
        [Result2]
    );

    public static readonly GeneralTestAnswer Q2Answer1 = new GeneralTestAnswer(
        Q2Answer1Id,
        GeneralTestAnswerTypeSpecificData.ImageAndText.CreateNew("Jupiter", "jupiter_image.png").GetSuccess(),
        [Result2]
    );

    public static readonly GeneralTestAnswer Q2Answer2 = new GeneralTestAnswer(
        Q2Answer2Id,
        GeneralTestAnswerTypeSpecificData.ImageAndText.CreateNew("Saturn", "saturn_image.png").GetSuccess(),
        [Result3]
    );

    public static readonly GeneralTestAnswer Q3Answer1 = new GeneralTestAnswer(
        Q3Answer1Id,
        GeneralTestAnswerTypeSpecificData.ColorOnly.CreateNew("#ff5733").GetSuccess(),
        [Result4]
    );

    public static readonly GeneralTestAnswer Q3Answer2 = new GeneralTestAnswer(
        Q3Answer2Id,
        GeneralTestAnswerTypeSpecificData.ColorOnly.CreateNew("#33ff57").GetSuccess(),
        [Result5]
    );

    public static readonly GeneralTestAnswer Q4Answer1 = new GeneralTestAnswer(
        Q4Answer1Id,
        GeneralTestAnswerTypeSpecificData.ColorOnly.CreateNew("#5733ff").GetSuccess(),
        [Result1]
    );

    public static readonly GeneralTestAnswer Q4Answer2 = new GeneralTestAnswer(
        Q4Answer2Id,
        GeneralTestAnswerTypeSpecificData.ColorOnly.CreateNew("#ff5733").GetSuccess(),
        [Result2]
    );

    public static readonly GeneralTestQuestion Question1 = new(
        new GeneralTestQuestionId(Guid.NewGuid()),
        orderInTest: 1,
        text: "What is the capital of France?",
        images: [],
        answersType: GeneralTestAnswersType.ImageAndText,
        answers: [Q1Answer1, Q1Answer2],
        shuffleAnswers: false,
        answersCountLimit: GeneralTestQuestionAnswersCountLimit.SingleChoice(),
        timeLimit: GeneralTestQuestionTimeLimitOption.HasTimeLimit(30).GetSuccess()
    );

    public static readonly GeneralTestQuestion Question2 = new(
        new GeneralTestQuestionId(Guid.NewGuid()),
        orderInTest: 2,
        text: "What is the largest planet in our solar system?",
        images: [],
        answersType: GeneralTestAnswersType.ImageAndText,
        answers: [Q2Answer1, Q2Answer2],
        shuffleAnswers: true,
        answersCountLimit: GeneralTestQuestionAnswersCountLimit.MultipleChoice(1, 2).GetSuccess(),
        timeLimit: GeneralTestQuestionTimeLimitOption.NoTimeLimit()
    );

    public static readonly GeneralTestQuestion Question3 = new(
        new GeneralTestQuestionId(Guid.NewGuid()),
        orderInTest: 3,
        text: "What is your favorite color?",
        images: [],
        answersType: GeneralTestAnswersType.ColorOnly,
        answers: [Q3Answer1, Q3Answer2],
        shuffleAnswers: false,
        answersCountLimit: GeneralTestQuestionAnswersCountLimit.MultipleChoice(1, 2).GetSuccess(),
        timeLimit: GeneralTestQuestionTimeLimitOption.NoTimeLimit()
    );

    public static readonly GeneralTestQuestion Question4 = new(
        new GeneralTestQuestionId(Guid.NewGuid()),
        orderInTest: 4,
        text: "What is your preferred color scheme?",
        images: [],
        answersType: GeneralTestAnswersType.ColorOnly,
        answers: [Q4Answer1, Q4Answer2],
        shuffleAnswers: true,
        answersCountLimit: GeneralTestQuestionAnswersCountLimit.SingleChoice(),
        timeLimit: GeneralTestQuestionTimeLimitOption.NoTimeLimit()
    );

    public static readonly AppUserId CreatorId = new AppUserId(Guid.NewGuid());
    public static readonly TestId TestId = new TestId(Guid.NewGuid());


    public static readonly ImmutableArray<GeneralTestQuestion> AllQuestions = [
        GeneralTestTestsConsts.Question1,
        GeneralTestTestsConsts.Question2,
        GeneralTestTestsConsts.Question3,
        GeneralTestTestsConsts.Question4
    ];

    public static readonly ImmutableArray<GeneralTestResult> AllResults = [
        GeneralTestTestsConsts.Result1,
        GeneralTestTestsConsts.Result2,
        GeneralTestTestsConsts.Result3,
        GeneralTestTestsConsts.Result4,
        GeneralTestTestsConsts.Result5
    ];

    public static GeneralFormatTest CreateTest(
        AccessLevel testAccessLevel = AccessLevel.Public,
        IReadOnlyCollection<GeneralTestQuestion> questions = null,
        IReadOnlyCollection<GeneralTestResult> results = null,
        GeneralTestFeedbackOption feedbackOption = null,
        bool shuffleQuestions = false
    ) => new(TestId, CreatorId, [], testAccessLevel,
        TestStylesSheet.CreateNew(TestId),
        questions ?? AllQuestions,
        shuffleQuestions,
        results ?? AllResults,
        feedbackOption ?? GeneralTestFeedbackOption.Disabled.Instance
    );

    public static readonly AppUserId TestTakerId = new(Guid.NewGuid());
    public static DateTime TestTakingStart => DateTime.Now.AddHours(-1);
    public static DateTime TestTakingEnd => DateTime.Now;

    public static IDateTimeProvider DateTimeProviderInstance = new UtcDateTimeProvider();
}