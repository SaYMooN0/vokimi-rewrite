using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands;

public record class UpdateTestCoverCommand(TestId TestId, string NewCoverImg) : IRequest<ErrOrNothing>;
public class UpdateTestCoverCommandHandler : IRequestHandler<UpdateTestCoverCommand, ErrOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public UpdateTestCoverCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(UpdateTestCoverCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        var updateRes = test.UpdateCoverImg(request.NewCoverImg);
        if (updateRes.IsErr(out var err)) {
            return err;
        }
        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}