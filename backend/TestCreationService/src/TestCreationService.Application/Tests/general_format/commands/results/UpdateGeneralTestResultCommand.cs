using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.results;

public record class UpdateGeneralTestResultCommand(
    TestId TestId,
    GeneralTestResultId ResultId,
    string Name,
    string Text,
    string Image
) : IRequest<ErrOrNothing>;
internal class UpdateGeneralTestResultCommandHandler :
    IRequestHandler<UpdateGeneralTestResultCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public UpdateGeneralTestResultCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(UpdateGeneralTestResultCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithResults(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var updateRes = test.UpdateResult(
            request.ResultId,
            request.Name,
            request.Text,
            request.Image ?? ""
        );
        if (updateRes.IsErr(out var err)) {
            return err;
        }
        await _generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
