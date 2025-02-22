using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Application.Tests.formats_shared.commands.ratings;

public record class UpdateTestRatingCommand(AppUserId UserId, TestId TestId, int Rating)
    : IRequest<ErrOr<ushort>>;

public class UpdateTestRatingCommandHandler : IRequestHandler<UpdateTestRatingCommand, ErrOr<ushort>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateTestRatingCommandHandler(
        IBaseTestsRepository baseTestsRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _baseTestsRepository = baseTestsRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<ushort>> Handle(UpdateTestRatingCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        if (request.Rating <= 0) {
            return Err.ErrFactory.InvalidData("Rating must be greater than 0");
        }

        if (request.Rating > ushort.MaxValue) {
            return Err.ErrFactory.InvalidData("Rating value is too big");
        }

        var updateRes = test.UpdateRating(request.UserId, (ushort)request.Rating, _dateTimeProvider);
        if (updateRes.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return (ushort)request.Rating;
    }
}