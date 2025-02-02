using MediatR;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Application.Tests.formats_shared.commands;

public record class UpdateTestInteractionsAccessSettingsCommand(
    TestId TestId,
    AccessLevel TestAccessLevel,
    ResourceAvailabilitySetting RatingsSetting,
    ResourceAvailabilitySetting DiscussionsSetting,
    bool AllowTestTakenPosts,
    ResourceAvailabilitySetting TagSuggestionsSetting
) : IRequest<ErrListOrNothing>;
public class UpdateTestInteractionsAccessSettingsCommandHandler
    : IRequestHandler<UpdateTestInteractionsAccessSettingsCommand, ErrListOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public UpdateTestInteractionsAccessSettingsCommandHandler(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task<ErrListOrNothing> Handle(UpdateTestInteractionsAccessSettingsCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }
        var updateRes = test.UpdateInteractionsAccessSettings(
            request.TestAccessLevel,
            ratingsSetting: request.RatingsSetting,
            discussionsSetting: request.DiscussionsSetting,
            request.AllowTestTakenPosts,
            request.TagSuggestionsSetting
        );
        if (updateRes.IsErr(out var err)) {
            return err;
        }
        await _baseTestsRepository.Update(test);
        return ErrListOrNothing.Nothing;
    }
}