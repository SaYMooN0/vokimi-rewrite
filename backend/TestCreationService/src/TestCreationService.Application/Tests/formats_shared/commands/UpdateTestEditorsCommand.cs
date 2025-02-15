using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands;

public record class UpdateTestEditorsCommand(TestId TestId, HashSet<AppUserId> EditorIds) : IRequest<ErrOr<ISet<AppUserId>>>;

public class UpdateTestEditorsCommandHandler : IRequestHandler<UpdateTestEditorsCommand, ErrOr<ISet<AppUserId>>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IAppUsersRepository _appUsersRepository;

    public UpdateTestEditorsCommandHandler(IBaseTestsRepository baseTestsRepository, IAppUsersRepository appUsersRepository) {
        _baseTestsRepository = baseTestsRepository;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOr<ISet<AppUserId>>> Handle(UpdateTestEditorsCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        foreach (AppUserId editorId in request.EditorIds) {
            AppUser? editor = await _appUsersRepository.GetById(editorId);
            if (editor is null) {
                return Err.ErrFactory.NotFound(
                    message: "Unknown editor user",
                    details: $"Creator with id {editorId} not found. Try to this user later"
                );
            }
        }
        test.UpdateEditors(request.EditorIds);
        await _baseTestsRepository.Update(test);
        return test.EditorIds;
    }
}