using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.publishing;

public record class PublishTierListTestCommand(TestId TestId) : IRequest<ErrOrNothing>;

public class PublishTierListTestCommandHandler : IRequestHandler<PublishTierListTestCommand, ErrOrNothing>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITierListFormatTestsRepository _tierListFormatTestsRepository;

    public PublishTierListTestCommandHandler(
        IDateTimeProvider dateTimeProvider,
        ITierListFormatTestsRepository tierListFormatTestsRepository
    ) {
        _dateTimeProvider = dateTimeProvider;
        _tierListFormatTestsRepository = tierListFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(PublishTierListTestCommand request, CancellationToken cancellationToken) {
        TierListFormatTest? test = await _tierListFormatTestsRepository.GetWithEverything(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

       
        return test.Publish(_dateTimeProvider);
    }
}