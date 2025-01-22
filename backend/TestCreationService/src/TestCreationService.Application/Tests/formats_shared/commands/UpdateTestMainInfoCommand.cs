using MediatR;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Net;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands;
public record class UpdateTestMainInfoCommand(
    TestId TestId,
    string TestName,
    string Description,
    Language Language
) : IRequest<ErrOrNothing>;

public class UpdateTestMainInfoCommandHandler : IRequestHandler<UpdateTestMainInfoCommand, ErrOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public UpdateTestMainInfoCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(UpdateTestMainInfoCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        var updateRes = test.UpdateMainInfo(request.TestName, request.Description, request.Language);
        if (updateRes.IsErr(out var err)) {
            return err;
        }
        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}