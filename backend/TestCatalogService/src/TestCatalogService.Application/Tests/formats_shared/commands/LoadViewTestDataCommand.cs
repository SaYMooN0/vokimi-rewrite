using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.general_format;

namespace TestCatalogService.Application.Tests.formats_shared.commands;

public record class LoadViewTestDataCommand(TestId TestId) : IRequest<ErrOr<BaseTest>>;

public class LoadViewTestDataCommandHandler : IRequestHandler<ErrOr<BaseTest>, GeneralFormatTest>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public LoadViewTestDataCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrOr<BaseTest>> Handle(LoadViewTestDataCommand request,
        CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return test;
    }
}