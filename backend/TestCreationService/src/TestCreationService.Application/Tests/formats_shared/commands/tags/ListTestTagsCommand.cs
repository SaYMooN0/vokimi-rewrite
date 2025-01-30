using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands.tags;

public record class ListTestTagsCommand(TestId TestId) : IRequest<ErrOr<ISet<string>>>;

public class ListTestTagsCommandHandler : IRequestHandler<ListTestTagsCommand, ErrOr<ISet<string>>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public ListTestTagsCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOr<ISet<string>>> Handle(ListTestTagsCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetWithTags(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        return ErrOr<ISet<string>>.Success(test.GetTags());
    }
}
