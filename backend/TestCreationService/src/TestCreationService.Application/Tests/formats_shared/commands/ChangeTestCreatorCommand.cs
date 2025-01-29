
using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands;


public record class ChangeTestCreatorCommand(TestId TestId, AppUserId NewCreatorId, bool KeepCurrentAsEditor) : IRequest<ErrOrNothing>;

public class ChangeTestCreatorCommandHandler : IRequestHandler<ChangeTestCreatorCommand, ErrOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IAppUsersRepository _appUsersRepository;

    public ChangeTestCreatorCommandHandler(IBaseTestsRepository baseTestsRepository, IAppUsersRepository appUsersRepository) {
        _baseTestsRepository = baseTestsRepository;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOrNothing> Handle(ChangeTestCreatorCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        var newCreator = await _appUsersRepository.GetById(request.NewCreatorId);
        if (newCreator is null) {
            return Err.ErrFactory.NotFound("User you want to set as creator not found", details: $"User with id {request.NewCreatorId} not found");
        }
        var updateRes = test.ChangeTestCreator(newCreator.Id, request.KeepCurrentAsEditor);
        if (updateRes.IsErr(out var err)) {
            return err;
        }
        await _baseTestsRepository.Update(test);

        return ErrOrNothing.Nothing;
    }
}