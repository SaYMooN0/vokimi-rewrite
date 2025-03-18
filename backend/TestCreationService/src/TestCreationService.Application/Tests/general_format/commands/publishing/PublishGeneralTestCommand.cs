using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.publishing;

public record class PublishGeneralTestCommand(TestId TestId) : IRequest<ErrOrNothing>;

public class PublishGeneralTestCommandHandler : IRequestHandler<PublishGeneralTestCommand, ErrOrNothing>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public PublishGeneralTestCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IGeneralFormatTestsRepository generalFormatTestsRepository,
        IGeneralTestQuestionsRepository generalTestQuestionsRepository
    ) {
        _dateTimeProvider = dateTimeProvider;
        _generalFormatTestsRepository = generalFormatTestsRepository;
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task<ErrOrNothing> Handle(PublishGeneralTestCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithEverything(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }

        var questions = await _generalTestQuestionsRepository.GetWithAnswersByIds(
            test.GetTestQuestionIds().ToHashSet()
        );
        
        return test.Publish(questions, _dateTimeProvider);
    }
}