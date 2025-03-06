using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Application.Tests.formats_shared.commands.tags.tag_suggestions;

public record DeclineAndBanTagSuggestionsForTestCommand(
    TestId TestId,
    HashSet<TestTagId> Tags
) : IRequest<ErrOrNothing>;

internal class DeclineAndBanTagSuggestionsForTestCommandHandler
    : IRequestHandler<DeclineAndBanTagSuggestionsForTestCommand, ErrOrNothing>

{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public DeclineAndBanTagSuggestionsForTestCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(
        DeclineAndBanTagSuggestionsForTestCommand request,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetWithTagSuggestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        var res = test.DeclineAndBanTagSuggestions(request.Tags);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}