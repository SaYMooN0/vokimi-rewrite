using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.value_objects;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands.styles;

public record class UpdateTestStylesCommand(
    TestId TestId,
    HexColor AccentColor,
    HexColor ErrorsColor,
    TestStylesButtons ButtonsStyles
) : IRequest<ErrOrNothing>;
public class UpdateTestStylesCommandHandler : IRequestHandler<UpdateTestStylesCommand, ErrOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    public UpdateTestStylesCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }
    public async Task<ErrOrNothing> Handle(UpdateTestStylesCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetWithStyles(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        test.UpdateStyles(request.AccentColor, request.ErrorsColor, request.ButtonsStyles);
        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}

