using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.Common.test_taken_data.general_format_test;
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
    private readonly IGeneralFormatTestsRepository _generalFormatRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GeneralTestTakenCommandHandler(
        IGeneralFormatTestsRepository generalFormatRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _generalFormatRepository = generalFormatRepository;
        _dateTimeProvider = dateTimeProvider;
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
            request.FeedbackData,
            _dateTimeProvider
        );
        if (testTakenRes.IsErr(out var err)) {
            return err;
        }

        await _generalFormatRepository.Update(test);
        return testTakenRes.GetSuccess();
    }
}