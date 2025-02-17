using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.results;

public record class CreateResultForGeneralTestCommand(
    TestId TestId
) : IRequest<ErrOr<GeneralTestResult>>;
internal class CreateResultForGeneralTestCommandCommandHandler :
    IRequestHandler<CreateResultForGeneralTestCommand, ErrOr<GeneralTestResult>>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public CreateResultForGeneralTestCommandCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<GeneralTestResult>> Handle(CreateResultForGeneralTestCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithResults(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var creationRes = test.CreateResult();
        if (creationRes.IsErr(out var err)) {
            return err;
        }
        await _generalFormatTestsRepository.Update(test);
        return creationRes.GetSuccess();
    }
}
