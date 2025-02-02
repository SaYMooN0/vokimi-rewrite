using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.general_format;
public record class CheckTestForPublishingProblemsCommand(TestId TestId) : IRequest<TestPublishingProblem[]>;

public class CheckTestForPublishingProblemsCommandHandler : IRequestHandler<CheckTestForPublishingProblemsCommand, TestPublishingProblem[]>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public CheckTestForPublishingProblemsCommandHandler(
        IBaseTestsRepository baseTestsRepository,
        IGeneralFormatTestsRepository generalFormatTestsRepository,
        IGeneralTestQuestionsRepository generalTestQuestionsRepository
    ) {
        _baseTestsRepository = baseTestsRepository;
        _generalFormatTestsRepository = generalFormatTestsRepository;
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task<TestPublishingProblem[]> Handle(CheckTestForPublishingProblemsCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return [new TestPublishingProblem("Test", "Unknown test", $"Test with id {request.TestId} not found")];
        }
        return test.Format switch {
            TestFormat.General => await CheckGeneralFormatTestForPublishingProblems(request.TestId),
            _ => throw new ErrCausedException(Err.ErrFactory.NotImplemented(
                $"Checking test for publishing problems is not implemented for the {test.Format} format test"
            ))
        };
    }
    private async Task<TestPublishingProblem[]> CheckGeneralFormatTestForPublishingProblems(TestId testId) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithEverything(testId);
        if (test is null) {
            return [new TestPublishingProblem("Test", "Unknown general format test", $"General format test with id {testId} not found")];
        }
        var questionsRes = await _generalTestQuestionsRepository.GetAllWithId(test.GetTestQuestionIds());
        if (questionsRes.IsErr(out var _)) {
            return [new TestPublishingProblem("Test", "Unable to get test questions", "Try again later. If it doesn't help try adding or removing one question")];
        }
        var questions = questionsRes.GetSuccess();
        return test.CheckForPublishingProblems(questions).ToArray();
    }
}