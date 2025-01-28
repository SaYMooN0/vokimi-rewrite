using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.results;

public record class DeleteGeneralTestResultCommand(
    TestId TestId,
    GeneralTestResultId ResultId
) : IRequest<ErrOrNothing>;
internal class DeleteGeneralTestResultCommandHandler :
    IRequestHandler<DeleteGeneralTestResultCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public DeleteGeneralTestResultCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(DeleteGeneralTestResultCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithResults(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var deleteRes = test.DeleteGeneralTestResult(request.ResultId);
        if (deleteRes.IsErr(out var err)) {
            return err;
        }
        await _generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
