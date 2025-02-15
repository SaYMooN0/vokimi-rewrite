using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands.styles;

public record class SetTestStylesDefaultCommand(TestId TestId) : IRequest<ErrOrNothing>;
public class SetTestStylesDefaultCommandHandler : IRequestHandler<SetTestStylesDefaultCommand, ErrOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    public SetTestStylesDefaultCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }
    public async Task<ErrOrNothing> Handle(SetTestStylesDefaultCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetWithStyles(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        test.SetStylesDefault();
        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}

