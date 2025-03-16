using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands;


public record class GetTierListTestFeedbackOptionCommand(
    TestId TestId
) : IRequest<ErrOr<TierListTestFeedbackOption>>;

public class GetTierListTestFeedbackOptionCommandHandler
    : IRequestHandler<GetTierListTestFeedbackOptionCommand, ErrOr<TierListTestFeedbackOption>>
{
    private readonly ITierListFormatTestsRepository _TierListFormatTestsRepository;

    public GetTierListTestFeedbackOptionCommandHandler(
        ITierListFormatTestsRepository TierListFormatTestsRepository
    ) {
        _TierListFormatTestsRepository = TierListFormatTestsRepository;
    }

    public async Task<ErrOr<TierListTestFeedbackOption>> Handle(
        GetTierListTestFeedbackOptionCommand request, CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _TierListFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return test.Feedback;
    }
}