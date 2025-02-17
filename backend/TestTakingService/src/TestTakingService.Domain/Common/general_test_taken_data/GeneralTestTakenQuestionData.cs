using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace TestTakingService.Domain.Common.general_test_taken_data;

public record class GeneralTestTakenQuestionData(  
    HashSet<GeneralTestAnswerId> ChosenAnswers,
    TimeSpan TimeOnQuestionSpent
);