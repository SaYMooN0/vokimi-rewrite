using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.publishing;

public record class CheckGeneralTestForPublishingProblemsCommand(
    TestId TestId
) : IRequest<ErrOr<TestPublishingProblem[]>>;

public class CheckGeneralTestForPublishingProblemsCommandHandler :
    IRequestHandler<CheckGeneralTestForPublishingProblemsCommand, ErrOr<TestPublishingProblem[]>>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public CheckGeneralTestForPublishingProblemsCommandHandler(
        IGeneralFormatTestsRepository generalFormatTestsRepository,
        IGeneralTestQuestionsRepository generalTestQuestionsRepository
    ) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task<ErrOr<TestPublishingProblem[]>> Handle(
        CheckGeneralTestForPublishingProblemsCommand request,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithEverything(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }

        var questions = await _generalTestQuestionsRepository
            .GetWithAnswersByIds(test.GetTestQuestionIds().ToHashSet()
        );

        return test.CheckForPublishingProblems(questions);
    }
}