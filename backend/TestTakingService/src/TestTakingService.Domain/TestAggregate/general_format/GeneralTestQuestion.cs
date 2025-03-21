using SharedKernel.Common.domain.entity;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralTestQuestion : Entity<GeneralTestQuestionId>
{
    private GeneralTestQuestion() { }
    public ushort OrderInTest { get; }
    public string Text { get; }
    public IReadOnlyCollection<string> Images { get; }
    public GeneralTestAnswersType AnswersType { get; }
    public IReadOnlyCollection<GeneralTestAnswer> Answers { get; }
    public bool ShuffleAnswers { get; }
    public GeneralTestQuestionAnswersCountLimit AnswersCountLimit { get; }
    public GeneralTestQuestionTimeLimitOption TimeLimit { get; }

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