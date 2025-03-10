using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using SharedUserRelationsContext.repository;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared.ratings;

namespace TestCatalogService.Application.Tests.formats_shared.commands.ratings;

public record class UpdateTestRatingCommand(AppUserId UserId, TestId TestId, int Rating)
    : IRequest<ErrOr<TestRating>>;

public class UpdateTestRatingCommandHandler : IRequestHandler<UpdateTestRatingCommand, ErrOr<TestRating>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserFollowingsRepository _userFollowingsRepository;


    public UpdateTestRatingCommandHandler(
        IBaseTestsRepository baseTestsRepository,
        IDateTimeProvider dateTimeProvider,
        IUserFollowingsRepository userFollowingsRepository
    ) {
        _baseTestsRepository = baseTestsRepository;
        _dateTimeProvider = dateTimeProvider;
        _userFollowingsRepository = userFollowingsRepository;
    }

    public async Task<ErrOr<TestRating>> Handle(UpdateTestRatingCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetWithRatings(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        if (request.Rating <= 0) {
            return Err.ErrFactory.InvalidData("Rating must be greater than 0");
        }

        if (request.Rating > ushort.MaxValue) {
            return Err.ErrFactory.InvalidData("Rating value is too big");
        }

        var updateRes = await test.UpdateRating(
            request.UserId,
            (ushort)request.Rating,
            _userFollowingsRepository,
            _dateTimeProvider
        );
        if (updateRes.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return updateRes.GetSuccess();
    }
}