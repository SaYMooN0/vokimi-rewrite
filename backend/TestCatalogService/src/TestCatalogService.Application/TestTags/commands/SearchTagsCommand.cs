using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common.interfaces.repositories;

namespace TestCatalogService.Application.TestTags.commands;

public record SearchTagsCommand(string Tag)
    : IRequest<ErrOr<ImmutableArray<TestTagId>>>;

public class SearchTagsToSuggestForTestCommandHandler :
    IRequestHandler<SearchTagsCommand, ErrOr<ImmutableArray<TestTagId>>>
{
    private readonly ITestTagsRepository _testTagsRepository;

    public SearchTagsToSuggestForTestCommandHandler(ITestTagsRepository testTagsRepository) {
        _testTagsRepository = testTagsRepository;
    }

    public async Task<ErrOr<ImmutableArray<TestTagId>>> Handle(
        SearchTagsCommand request, CancellationToken cancellationToken
    ) {
        if (!TestTagsRules.IsStringValidTag(request.Tag)) {
            return Err.ErrFactory.InvalidData($"{request.Tag} is an incorrect value");
        }
        
    }
}