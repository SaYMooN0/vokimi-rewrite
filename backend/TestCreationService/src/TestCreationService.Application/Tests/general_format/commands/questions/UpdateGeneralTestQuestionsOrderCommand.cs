using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class UpdateGeneralTestQuestionsOrderCommand(
    TestId TestId,
    EntitiesOrderController<GeneralTestQuestionId> OrderController
) : IRequest<ErrOrNothing>;
internal class UpdateGeneralTestQuestionsOrderCommandHandler : IRequestHandler<UpdateGeneralTestQuestionsOrderCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public UpdateGeneralTestQuestionsOrderCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(UpdateGeneralTestQuestionsOrderCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var updateErr = test.UpdateQuestionsOrder(request.OrderController);
        if (updateErr.IsErr(out var err)) { return err; }
        await _generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
