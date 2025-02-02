using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;
namespace TestCreationService.Application.Tests.formats_shared.commands.tags;
public record class ClearTestTagsCommand(TestId TestId) : IRequest<ErrOrNothing>;

public class ClearTestTagsCommandHandler : IRequestHandler<ClearTestTagsCommand, ErrOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public ClearTestTagsCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(ClearTestTagsCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetWithTags(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        test.ClearTags();
        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
