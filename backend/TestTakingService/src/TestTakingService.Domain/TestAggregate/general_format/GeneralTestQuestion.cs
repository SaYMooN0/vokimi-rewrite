using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralTestQuestion : Entity<GeneralTestQuestionId>
{
    private GeneralTestQuestion() { }
    private TestId _testId { get; init; }
    public ushort OrderInTest { get; init; }
    public string Text { get; init; }
    public ImmutableArray<string> Images { get; init; }
    protected virtual ImmutableArray<GeneralTestAnswer> _answers { get; init; }
    private bool _shuffledAnswers { get; init; }
    public GeneralTestQuestionAnswersCountLimit AnswersCountLimit { get; init; }
    public GeneralTestQuestionTimeLimitOption TimeLimit { get; init; }

    public GeneralTestQuestion(
        GeneralTestQuestionId questionId,
        TestId testId,
        ushort orderInTest,
        string text,
        ImmutableArray<string> images,
        ImmutableArray<GeneralTestAnswer> answers,
        bool shuffledAnswers,
        GeneralTestQuestionAnswersCountLimit answersCountLimit,
        GeneralTestQuestionTimeLimitOption timeLimit
    ) {
        Id = questionId;
        _testId = testId;
        OrderInTest = orderInTest;
        Text = text;
        Images = images;
        _answers = answers;
        _shuffledAnswers = shuffledAnswers;
        AnswersCountLimit = answersCountLimit;
        TimeLimit = timeLimit;
    }
}