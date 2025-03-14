using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using SharedKernel.Common.tests.value_objects;
using TestAnswer = TestTakingService.Domain.TestAggregate.general_format.GeneralTestAnswer;
using TestQuestion = TestTakingService.Domain.TestAggregate.general_format.GeneralTestQuestion;
using TestResult = TestTakingService.Domain.TestAggregate.general_format.GeneralTestResult;

namespace DBSeeder.Data.published_tests.published_tests_data.instances;

public class GeneralFormatPublishedTestsDataTest1
{
    private static readonly TestResult Result1 = new(
        new(Guid.NewGuid()), "Explorer",
        "You are an explorer, always seeking new knowledge!", "explorer.png"
    );

    private static readonly TestResult Result2 = new(
        new(Guid.NewGuid()), "Scientist",
        "You have a scientific mind, always analyzing and discovering!", "scientist.png"
    );

    private static readonly TestResult Result3 = new(
        new(Guid.NewGuid()), "Philosopher",
        "You think deeply about the world and seek wisdom.", "philosopher.png"
    );

    private static readonly TestResult Result4 = new(
        new(Guid.NewGuid()),
        name: "Artist", "You have a creative mind, always seeing beauty in the world.", "artist.png"
    );

    private static readonly TestResult Result5 = new(
        new(Guid.NewGuid()), "Mathematician",
        "You have a logical mind and are always solving complex problems.", "mathematician.png"
    );


    private static readonly TestAnswer Q1Answer1 = new TestAnswer(
        new(Guid.NewGuid()),
        GeneralTestAnswerTypeSpecificData.ImageAndText.CreateNew("Paris", "paris_image.png").GetSuccess(),
        [Result1]
    );

    private static readonly TestAnswer Q1Answer2 = new TestAnswer(
        new(Guid.NewGuid()),
        GeneralTestAnswerTypeSpecificData.ImageAndText.CreateNew("London", "london_image.png").GetSuccess(),
        [Result2]
    );

    private static readonly TestAnswer Q2Answer1 = new TestAnswer(
        new(Guid.NewGuid()),
        GeneralTestAnswerTypeSpecificData.ImageAndText.CreateNew("Jupiter", "jupiter_image.png").GetSuccess(),
        [Result2]
    );

    private static readonly TestAnswer Q2Answer2 = new TestAnswer(
        new(Guid.NewGuid()),
        GeneralTestAnswerTypeSpecificData.ImageAndText.CreateNew("Saturn", "saturn_image.png").GetSuccess(),
        [Result3]
    );

    private static readonly TestAnswer Q3Answer1 = new TestAnswer(
        new(Guid.NewGuid()),
        GeneralTestAnswerTypeSpecificData.ColorOnly.CreateNew("#ff5733").GetSuccess(),
        [Result4]
    );

    private static readonly TestAnswer Q3Answer2 = new TestAnswer(
        new(Guid.NewGuid()),
        GeneralTestAnswerTypeSpecificData.ColorOnly.CreateNew("#33ff57").GetSuccess(),
        [Result5]
    );

    private static readonly TestAnswer Q4Answer1 = new TestAnswer(
        new(Guid.NewGuid()),
        GeneralTestAnswerTypeSpecificData.ColorOnly.CreateNew("#5733ff").GetSuccess(),
        [Result1]
    );

    private static readonly TestAnswer Q4Answer2 = new TestAnswer(
        new(Guid.NewGuid()),
        GeneralTestAnswerTypeSpecificData.ColorOnly.CreateNew("#ff5733").GetSuccess(),
        [Result2]
    );

    private static readonly TestQuestion Question1 = new(
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

    private static readonly TestQuestion Question2 = new(
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

    private static readonly TestQuestion Question3 = new(
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

    private static readonly TestQuestion Question4 = new(
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
    public static readonly ImmutableArray<TestQuestion> AllQuestions = [
        Question1,
        Question2,
        Question3,
        Question4
    ];

    public static readonly ImmutableArray<TestResult> AllResults = [
        Result1,
        Result2,
        Result3,
        Result4,
        Result5
    ];
}