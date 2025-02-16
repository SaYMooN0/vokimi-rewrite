using MediatR;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCatalogService.Application.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Application.Tests.formats_shared.commands;

public record class LoadViewTestDataCommand(TestId TestId) : IRequest<ErrOr<BaseTest>>;

public class LoadViewTestDataCommandHandler : IRequestHandler<LoadViewTestDataCommand, ErrOr<BaseTest>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public LoadViewTestDataCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOr<BaseTest>> Handle(LoadViewTestDataCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return test;
    }
}