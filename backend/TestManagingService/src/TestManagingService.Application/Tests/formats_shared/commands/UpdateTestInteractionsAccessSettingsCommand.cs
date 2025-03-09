using MediatR;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.formats_shared;

namespace TestManagingService.Application.Tests.formats_shared.commands;

public record class UpdateTestInteractionsAccessSettingsCommand(
    TestId TestId,
    AccessLevel TestAccessLevel,
    ResourceAvailabilitySetting RatingsSetting,
    ResourceAvailabilitySetting CommentsSetting,
    bool AllowTestTakenPosts,
    bool AllowTagSuggestions
) : IRequest<ErrListOr<TestInteractionsAccessSettings>>;

public class UpdateTestInteractionsAccessSettingsCommandHandler
    : IRequestHandler<UpdateTestInteractionsAccessSettingsCommand, ErrListOr<TestInteractionsAccessSettings>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public UpdateTestInteractionsAccessSettingsCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrListOr<TestInteractionsAccessSettings>> Handle(
        UpdateTestInteractionsAccessSettingsCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return new ErrList(Err.ErrPresets.TestNotFound(request.TestId));
        }

        var updateRes = test.UpdateInteractionsAccessSettings(
            request.TestAccessLevel,
            ratingsSetting: request.RatingsSetting,
            commentsSetting: request.CommentsSetting,
            request.AllowTestTakenPosts,
            request.AllowTagSuggestions
        );
        if (updateRes.AnyErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return updateRes.GetSuccess();
    }
}