using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Application.Tests.general_format;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.TestAggregate.scoring_format;

namespace TestCreationService.Application.Tests.scoring_format.commands;

public record class InitScoringFormatTestCommand(
    string TestName,
    AppUserId CreatorId,
    HashSet<AppUserId> EditorIds
) : IRequest<ErrOr<TestId>>;

public class InitGeneralFormatTestCommandHandler : IRequestHandler<InitScoringFormatTestCommand, ErrOr<TestId>>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IScoringFormatTestsRepository _scoringFormatTestsRepository;

    public InitGeneralFormatTestCommandHandler(IAppUsersRepository appUsersRepository, IScoringFormatTestsRepository scoringFormatTestsRepository) {
        _appUsersRepository = appUsersRepository;
        _scoringFormatTestsRepository = scoringFormatTestsRepository;
    }

    public async Task<ErrOr<TestId>> Handle(InitScoringFormatTestCommand request, CancellationToken cancellationToken) {
        var newTestCreationRes = ScoringFormatTest.CreateNew(
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
        await _scoringFormatTestsRepository.AddNew(newTest);
        return newTest.Id;
    }
}
