using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Tests.general_format.commands;

public record class GeneralTestTakenCommand(
    TestId TestId,
    AppUserId? TestTakerId,
    Dictionary<GeneralTestQuestionId, HashSet<GeneralTestAnswerId>> ChosenAnswers,
    GeneralTestTakenFeedbackData FeedbackData
) : IRequest<ErrOr<GeneralFormatTest>>;

public class GeneralTestTakenCommandHandler
    : IRequestHandler<GeneralTestTakenCommand, ErrOr<GeneralFormatTest>>
{
    private IGeneralFormatTestsRepository _generalFormatRepository;

    public async Task<ErrOr<GeneralFormatTest>> Handle(
        GeneralTestTakenCommand request,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatRepository.GetWithQuestionsAnswersAndResults(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }

        return Err.ErrFactory.NotImplemented();
        // var testTakenRes = test.TestTaken();
        // await _generalFormatRepository.Add()
        return ErrOr<GeneralFormatTest>.Success(test);
    }
}