using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common.interfaces.repositories;

namespace TestCatalogService.Application.TestTags.commands;

public record SearchTagValuesCommand(string Tag)
    : IRequest<ErrOr<ImmutableArray<string>>>;

public class SearchTagsToSuggestForTestCommandHandler :
    IRequestHandler<SearchTagValuesCommand, ErrOr<ImmutableArray<string>>>
{
    private readonly ITestTagsRepository _testTagsRepository;

    public SearchTagsToSuggestForTestCommandHandler(ITestTagsRepository testTagsRepository) {
        _testTagsRepository = testTagsRepository;
    }

    public async Task<ErrOr<ImmutableArray<string>>> Handle(
        SearchTagValuesCommand request, CancellationToken cancellationToken
    ) {
        if (!TestTagsRules.IsStringValidTag(request.Tag)) {
            return Err.ErrFactory.InvalidData($"{request.Tag} is an incorrect value");
        }

        var tags = await _testTagsRepository.TagIdValuesWithSubstring(request.Tag, 20);

        return ErrOr<ImmutableArray<string>>.Success([request.Tag, ..tags]);
    }
}