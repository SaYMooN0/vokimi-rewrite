using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Application.Tests.formats_shared.commands.tags;

public record ListTestTagsCommand(
    TestId TestId
) : IRequest<ErrOr<ImmutableArray<TestTagId>>>;

internal class ListTestTagsCommandHandler
    : IRequestHandler<ListTestTagsCommand, ErrOr<ImmutableArray<TestTagId>>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public ListTestTagsCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOr<ImmutableArray<TestTagId>>> Handle(
        ListTestTagsCommand request,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetWithTagSuggestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return test.Tags;
    }
}