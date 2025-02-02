using MediatR;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.AppUserAggregate;
using SharedKernel.Common.domain;

namespace TestCreationService.Application.Tests.general_format.commands;

public record class InitGeneralFormatTestCommand(
    string TestName,
    AppUserId CreatorId,
    HashSet<AppUserId> EditorIds
) : IRequest<ErrOr<TestId>>;


public class InitGeneralFormatTestCommandHandler : IRequestHandler<InitGeneralFormatTestCommand, ErrOr<TestId>>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public InitGeneralFormatTestCommandHandler(
        IAppUsersRepository appUsersRepository,
        IGeneralFormatTestsRepository generalFormatTestsRepository
    ) {
        _appUsersRepository = appUsersRepository;
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<TestId>> Handle(InitGeneralFormatTestCommand request, CancellationToken cancellationToken) {
        var newTestCreationRes = GeneralFormatTest.CreateNew(
            request.CreatorId,
            request.TestName,
            request.EditorIds
        );
        if (newTestCreationRes.IsErr(out var err)) { return err; }

        var newTest = newTestCreationRes.GetSuccess();

        AppUser? creator = await _appUsersRepository.GetById(request.CreatorId);
        if (creator is null) {
            return Err.ErrFactory.NotFound(
                message: "Unknown creator user",
                details: "User not found. Try log out and log in again."
            );
        }
        foreach (var editorId in newTest.EditorIds) {
            AppUser? editor = await _appUsersRepository.GetById(editorId);
            if (editor is null) {
                return Err.ErrFactory.NotFound(
                    message: "Unknown editor user",
                    details: $"Creator with id {editorId} not found"
                );
            }
        }
        await _generalFormatTestsRepository.AddNew(newTest);
        return newTest.Id;
    }
}
