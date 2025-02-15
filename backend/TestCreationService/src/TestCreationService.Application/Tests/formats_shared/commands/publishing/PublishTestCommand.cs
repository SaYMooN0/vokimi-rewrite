using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.general_format;
public record class PublishTestCommand(TestId TestId) : IRequest<ErrOrNothing>;

public class PublishTestCommandHandler : IRequestHandler<PublishTestCommand, ErrOrNothing>
{
    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly IBaseTestsRepository _baseTestsRepository;

    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public PublishTestCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IBaseTestsRepository baseTestsRepository,
        IGeneralFormatTestsRepository generalFormatTestsRepository,
        IGeneralTestQuestionsRepository generalTestQuestionsRepository
    ) {
        _dateTimeProvider = dateTimeProvider;
        _baseTestsRepository = baseTestsRepository;
        _generalFormatTestsRepository = generalFormatTestsRepository;
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task<ErrOrNothing> Handle(PublishTestCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        return test.Format switch {
            TestFormat.General => await PublishGeneralFormatTest(request.TestId),
            _ => throw new ErrCausedException(Err.ErrFactory.NotImplemented(
                $"Publishing test is not implemented for the {test.Format} format test"
            ))
        };
    }
    private async Task<ErrOrNothing> PublishGeneralFormatTest(TestId testId) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithEverything(testId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(testId);
        }
        var questionsRes = await _generalTestQuestionsRepository.GetAllWithId(test.GetTestQuestionIds());
        if (questionsRes.IsErr(out var _)) {
            return Err.ErrFactory.NotFound("Unable to get all test questions", "Try again later. If it doesn't help try adding or removing one question");
        }
        var questions = questionsRes.GetSuccess();
        return test.Publish(questions, _dateTimeProvider);
    }
}