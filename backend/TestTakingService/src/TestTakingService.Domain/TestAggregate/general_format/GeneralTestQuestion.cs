using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralTestQuestion : Entity<GeneralTestQuestionId>
{
    private GeneralTestQuestion() { }

    public ushort OrderInTest { get; init; }
    public string Text { get; init; }
    public IReadOnlyCollection<string> Images { get; init; }
    public GeneralTestAnswersType AnswersType { get; init; }
    public IReadOnlyCollection<GeneralTestAnswer> Answers { get; init; }
    public bool ShuffleAnswers { get; init; }
    public GeneralTestQuestionAnswersCountLimit AnswersCountLimit { get; init; }
    public GeneralTestQuestionTimeLimitOption TimeLimit { get; init; }

    public GeneralTestQuestion(
        GeneralTestQuestionId questionId,
        ushort orderInTest,
        string text,
        IReadOnlyCollection<string> images,
        GeneralTestAnswersType answersType,
        IReadOnlyCollection<GeneralTestAnswer> answers,
        bool shuffleAnswers,
        GeneralTestQuestionAnswersCountLimit answersCountLimit,
        GeneralTestQuestionTimeLimitOption timeLimit
    ) {
        Id = questionId;
        OrderInTest = orderInTest;
        Text = text;
        Images = images;
        AnswersType = answersType;
        Answers = answers;
        ShuffleAnswers = shuffleAnswers;
        AnswersCountLimit = answersCountLimit;
        TimeLimit = timeLimit;
    }
}