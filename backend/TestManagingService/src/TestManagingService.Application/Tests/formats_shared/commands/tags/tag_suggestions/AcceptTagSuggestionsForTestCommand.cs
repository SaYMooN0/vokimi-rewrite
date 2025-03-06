using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Application.Tests.formats_shared.commands.tags.tag_suggestions;

public record AcceptTagSuggestionsForTestCommand(
    TestId TestId,
    HashSet<TestTagId> Tags
) : IRequest<ErrOrNothing>;

internal class AcceptTagSuggestionsForTestCommandHandler
    : IRequestHandler<AcceptTagSuggestionsForTestCommand, ErrOrNothing>

{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public AcceptTagSuggestionsForTestCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(
        AcceptTagSuggestionsForTestCommand request,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetWithTagSuggestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        var res = test.AcceptTagSuggestions(request.Tags);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}