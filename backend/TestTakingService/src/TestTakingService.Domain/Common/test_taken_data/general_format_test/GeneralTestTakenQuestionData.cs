using SharedKernel.Common.domain.entity;

namespace TestTakingService.Domain.Common.test_taken_data.general_format_test;

public record class GeneralTestTakenQuestionData(  
    HashSet<GeneralTestAnswerId> ChosenAnswers,
    TimeSpan TimeOnQuestionSpent
);