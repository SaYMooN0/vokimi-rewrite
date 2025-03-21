using ApiShared.interfaces;
using SharedKernel.Common.errors;

namespace TestTakingService.Api.Contracts.test_taken.test_formats_shared;

public abstract class TestTakenRequest : IRequestWithValidationNeeded
{
    public abstract DateTime StartDateTime { get; init; }
    public abstract DateTime EndDateTime { get; init; }
    public abstract RequestValidationResult Validate();

    protected ErrOrNothing ValidateStartAndEndDateTine() =>
        StartDateTime >= EndDateTime
            ? Err.ErrFactory.InvalidData("Start date must be before end date")
            : ErrOrNothing.Nothing;
}