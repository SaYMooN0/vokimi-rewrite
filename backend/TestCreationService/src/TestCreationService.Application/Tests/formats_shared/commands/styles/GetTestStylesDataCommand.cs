using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands.styles;

public record class GetTestStylesDataCommand(TestId TestId) : IRequest<ErrOrNothing>;
//styles data
public class GetTestStylesDataCommandHandler : IRequestHandler<GetTestStylesDataCommand, ErrOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    public GetTestStylesDataCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }
    public async Task<ErrOrNothing> Handle(GetTestStylesDataCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetWithStyles(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        return Err.ErrFactory.NotImplemented();
    }
}

