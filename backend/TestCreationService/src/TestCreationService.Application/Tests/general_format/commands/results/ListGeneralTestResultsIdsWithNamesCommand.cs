using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Collections.Immutable;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.results;

public record class ListGeneralTestResultsIdsWithNamesCommand(
    TestId TestId
) : IRequest<ErrOr<ImmutableDictionary<GeneralTestResultId, string>>>;
internal class ListGeneralTestResultsIdsWithNamesCommandHandler :
    IRequestHandler<ListGeneralTestResultsIdsWithNamesCommand, ErrOr<ImmutableDictionary<GeneralTestResultId, string>>>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public ListGeneralTestResultsIdsWithNamesCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<ImmutableDictionary<GeneralTestResultId, string>>> Handle(ListGeneralTestResultsIdsWithNamesCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithResults(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        return test.GetTestResultIdsWithNames();
    }
}
