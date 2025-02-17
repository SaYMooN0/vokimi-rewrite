using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Tests.general_format.commands;

public record class LoadGeneralTestTakingDataCommand(TestId TestId) : IRequest<ErrOr<GeneralFormatTest>>;

public class LoadGeneralTestTakingDataCommandHandler
    : IRequestHandler<LoadGeneralTestTakingDataCommand, ErrOr<GeneralFormatTest>>
{
    private IGeneralFormatTestsRepository _generalFormatRepository;

    public LoadGeneralTestTakingDataCommandHandler(IGeneralFormatTestsRepository generalFormatRepository) {
        _generalFormatRepository = generalFormatRepository;
    }

    public async Task<ErrOr<GeneralFormatTest>> Handle(
        LoadGeneralTestTakingDataCommand request,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatRepository.GetWithQuestionWithAnswers(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }

        return ErrOr<GeneralFormatTest>.Success(test);
    }
}