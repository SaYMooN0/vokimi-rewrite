using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Application.Tests.formats_shared.commands.tags.tag_suggestions;

public record AddTagSuggestionsForTestCommand(
    TestId TestId,
    HashSet<TestTagId> Tags
) : IRequest<ErrOrNothing>;

internal class AddTagSuggestionsForTestCommandHandler
    : IRequestHandler<AddTagSuggestionsForTestCommand, ErrOrNothing>

{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AddTagSuggestionsForTestCommandHandler(
        IBaseTestsRepository baseTestsRepository, IDateTimeProvider dateTimeProvider
    ) {
        _baseTestsRepository = baseTestsRepository;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<ErrOrNothing> Handle(
        AddTagSuggestionsForTestCommand request,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetWithTagSuggestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        var addingRes = test.AddTagSuggestions(request.Tags, _dateTimeProvider);
        if (addingRes.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}