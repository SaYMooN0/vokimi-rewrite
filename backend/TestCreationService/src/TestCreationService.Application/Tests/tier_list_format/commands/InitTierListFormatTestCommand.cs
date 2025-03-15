using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands;

public record class InitTierListFormatTestCommand(
    string TestName,
    AppUserId CreatorId,
    HashSet<AppUserId> EditorIds
) : IRequest<ErrOr<TestId>>;

public class InitTierListFormatTestCommandHandler
    : IRequestHandler<InitTierListFormatTestCommand, ErrOr<TestId>>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly ITierListFormatTestsRepository _tierListFormatRepository;

    public InitTierListFormatTestCommandHandler(
        IAppUsersRepository appUsersRepository, ITierListFormatTestsRepository tierListFormatRepository
    ) {
        _appUsersRepository = appUsersRepository;
        _tierListFormatRepository = tierListFormatRepository;
    }

    public async Task<ErrOr<TestId>> Handle(
        InitTierListFormatTestCommand request, CancellationToken cancellationToken
    ) {
        var newTestCreationRes = TierListFormatTest.CreateNew(
            request.CreatorId,
            request.TestName,
            request.EditorIds
        );
        if (newTestCreationRes.IsErr(out var err)) {
            return err;
        }

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

        await _tierListFormatRepository.AddNew(newTest);
        return newTest.Id;
    }
}