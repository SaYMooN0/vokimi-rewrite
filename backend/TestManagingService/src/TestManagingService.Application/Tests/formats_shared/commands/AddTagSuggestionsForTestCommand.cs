using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Application.Tests.formats_shared.commands;

public record AddTagSuggestionsForTestCommand(
    TestId TestId,
    HashSet<TestTagId> Tags
) : IRequest<ErrOrNothing>;

public class AddTagSuggestionsForTestCommandHandler
    : IRequestHandler<AddTagSuggestionsForTestCommand, ErrOrNothing>

{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public AddTagSuggestionsForTestCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }


    public async Task<ErrOrNothing> Handle(
        AddTagSuggestionsForTestCommand request,
        CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        var addingRes = test.AddTagSuggestions(request.Tags);
        if (addingRes.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}