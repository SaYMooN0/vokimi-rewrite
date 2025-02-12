using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.Common.general_test_taken_data;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Tests.general_format.commands;

public record class GeneralTestTakenCommand(
    TestId TestId,
    AppUserId? TestTakerId,
    Dictionary<GeneralTestQuestionId, GeneralTestTakenQuestionData> QuestionsData,
    GeneralTestTakenFeedbackData? FeedbackData,
    DateTime TestTakingStart,
    DateTime TestTakingEnd
) : IRequest<ErrOr<GeneralTestResult>>;

public class GeneralTestTakenCommandHandler
    : IRequestHandler<GeneralTestTakenCommand, ErrOr<GeneralTestResult>>
{
    private IGeneralFormatTestsRepository _generalFormatRepository;
    public GeneralTestTakenCommandHandler(IGeneralFormatTestsRepository generalFormatRepository) {
        _generalFormatRepository = generalFormatRepository;
    }

    public async Task<ErrOr<GeneralTestResult>> Handle(
        GeneralTestTakenCommand request,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatRepository.GetWithQuestionsAnswersAndResults(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }

        var testTakenRes = test.TestTaken(
            request.TestTakerId,
            request.QuestionsData,
            testTakingStart: request.TestTakingStart,
            testTakingEnd: request.TestTakingEnd,
            request.FeedbackData
        );
        if (testTakenRes.IsErr(out var err)) {
            return err;
        }

        return testTakenRes.GetSuccess();
    }
}