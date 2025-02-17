using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Tests.general_format.commands;

public record class LoadAllResultsForGeneralTestCommand(
    TestId TestId
) : IRequest<ErrOr<IReadOnlyCollection<GeneralTestResult>>>;

public class LoadAllResultsForGeneralTestCommandHandler
    : IRequestHandler<LoadAllResultsForGeneralTestCommand, ErrOr<IReadOnlyCollection<GeneralTestResult>>>
{
    private IGeneralFormatTestsRepository _generalFormatRepository;

    public async Task<ErrOr<IReadOnlyCollection<GeneralTestResult>>> Handle(
        LoadAllResultsForGeneralTestCommand request,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatRepository.GetWithResults(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        return ErrOr<IReadOnlyCollection<GeneralTestResult>>.Success(test.Results);
    }
}