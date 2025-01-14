using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;

namespace TestCreationService.Application.Tests.formats_shared.commands;

public record class UpdateTestEditorsCommand(TestId TestId, IEnumerable<AppUserId> EditorIds)
    : IRequest<ErrOr<HashSet<AppUserId>>>;

public class UpdateTestEditorsCommandHandler : IRequestHandler<UpdateTestEditorsCommand, ErrOr<HashSet<AppUserId>>>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;
    public async Task<ErrOr<HashSet<AppUserId>>> Handle(UpdateTestEditorsCommand request, CancellationToken cancellationToken) {
        return Err.ErrFactory.NotImplemented();
    }
}