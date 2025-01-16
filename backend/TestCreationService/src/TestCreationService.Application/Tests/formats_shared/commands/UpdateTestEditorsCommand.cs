using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands;

public record class UpdateTestEditorsCommand(TestId TestId, HashSet<AppUserId> EditorIds)
    : IRequest<ErrOr<HashSet<AppUserId>>>;

public class UpdateTestEditorsCommandHandler : IRequestHandler<UpdateTestEditorsCommand, ErrOr<HashSet<AppUserId>>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    public async Task<ErrOr<HashSet<AppUserId>>> Handle(UpdateTestEditorsCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrFactory.NotFound("Test not found");
        }
        test.UpdateTestEditors(request.EditorIds);
        await _baseTestsRepository.Update(test);
        return request.EditorIds;
    }
}