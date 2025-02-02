using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands.tags;

public record class UpdateTestTagsCommand(TestId TestId, IEnumerable<string> NewTags) : IRequest<ErrListOr<ISet<string>>>;

public class UpdateTestTagsCommandHandler : IRequestHandler<UpdateTestTagsCommand, ErrListOr<ISet<string>>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public UpdateTestTagsCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrListOr<ISet<string>>> Handle(UpdateTestTagsCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetWithTags(request.TestId);
        if (test is null) {
            return new ErrList(Err.ErrPresets.TestNotFound(request.TestId));
        }
        var updateRes = test.UpdateTags(request.NewTags);
        if (updateRes.AnyErr(out var errlist)) {
            return errlist;
        }
        await _baseTestsRepository.Update(test);
        return ErrListOr<ISet<string>>.Success(updateRes.GetSuccess());
    }
}
