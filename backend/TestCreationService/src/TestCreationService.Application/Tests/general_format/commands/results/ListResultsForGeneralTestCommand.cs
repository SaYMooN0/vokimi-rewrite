using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using System.Collections.Immutable;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.results;


public record class ListResultsForGeneralTestCommand(
    TestId TestId
) : IRequest<ErrOr<ImmutableArray<GeneralTestResult>>>;
internal class ListResultsForGeneralTestCommandHandler :
    IRequestHandler<ListResultsForGeneralTestCommand, ErrOr<ImmutableArray<GeneralTestResult>>>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public ListResultsForGeneralTestCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<ImmutableArray<GeneralTestResult>>> Handle(ListResultsForGeneralTestCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithResults(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        return test.Results;
    }
}

