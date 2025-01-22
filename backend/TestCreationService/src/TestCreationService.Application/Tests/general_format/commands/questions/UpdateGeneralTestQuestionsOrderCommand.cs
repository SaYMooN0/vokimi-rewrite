using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class UpdateGeneralTestQuestionsOrderCommand(
    TestId TestId,
    EntitiesOrderController<GeneralTestQuestionId> OrderController
) : IRequest<ErrOrNothing>;
public class UpdateGeneralTestQuestionsOrderCommandHandler : IRequestHandler<UpdateGeneralTestQuestionsOrderCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public UpdateGeneralTestQuestionsOrderCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(UpdateGeneralTestQuestionsOrderCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithQuestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var updateErr = test.UpdateQuestionsOrder(request.OrderController);
        if (updateErr.IsErr(out var err)) { return err; }
        await _generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
