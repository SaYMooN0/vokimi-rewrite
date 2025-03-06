using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.formats_shared;

namespace TestManagingService.Application.Tests.formats_shared.commands.tags.tag_suggestions;

public record ListTagSuggestionsForTestCommand(
    TestId TestId,
    HashSet<TestTagId> Tags
) : IRequest<ErrOr<ImmutableArray<TagSuggestionForTest>>>;

internal class ListTagSuggestionsForTestCommandHandler
    : IRequestHandler<ListTagSuggestionsForTestCommand, ErrOr<ImmutableArray<TagSuggestionForTest>>>

{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public ListTagSuggestionsForTestCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOr<ImmutableArray<TagSuggestionForTest>>> Handle(
        ListTagSuggestionsForTestCommand request,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetWithTagSuggestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return test.TagSuggestions;
    }
}