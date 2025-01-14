using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.Common.EntityIds;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.AppUserAggregate;

namespace TestCreationService.Application.Tests.general_format;

public record class InitGeneralFormatTestCommand(
    string TestName,
    AppUserId CreatorId,
    AppUserId[] EditorIds
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
        var newTestCreationRes = GeneralFormatTest.CreateNew(request.CreatorId, request.TestName, request.EditorIds);
        if (newTestCreationRes.IsErr(out var err)) { return err; }
        AppUser? creator = await _appUsersRepository.GetById(request.CreatorId);
        if (creator is null) {
            return Err.ErrFactory.NotFound(message: "Unknown user", details: "User not found. Try log out and log in again.");
        }
        var newTest = newTestCreationRes.GetSuccess();
        await _generalFormatTestsRepository.AddNew(newTest);
        creator.AddCreatedTest(newTest.Id);
        return newTest.Id;
    }
}
