using Microsoft.AspNetCore.Http;
using SharedKernel.Common.errors;

namespace ApiShared;

public class CustomResults
{
    public record class ErrorResponseObject(Err[] errors);
    public static IResult ErrorResponse(Err error) =>
        (error.Source == ErrorSource.Server || error.Source == ErrorSource.ThirdParty) ?
        Results.InternalServerError(new ErrorResponseObject([error])) :
        Results.BadRequest(new ErrorResponseObject([error]));
    public static IResult ErrorResponse(ErrList error) => ErrorResponse(error.ToArray());
    public static IResult ErrorResponse(Err[] errors) {

        var hasClientError = errors.Any(err => err.Source == ErrorSource.Client);
        if (hasClientError) {
            return Results.BadRequest(new ErrorResponseObject(errors));
        }

        var allServerOrThirdParty = errors.All(err => err.Source == ErrorSource.Server || err.Source == ErrorSource.ThirdParty);
        if (allServerOrThirdParty) {
            return Results.InternalServerError(new ErrorResponseObject(errors));
        }

        return Results.BadRequest(new ErrorResponseObject(errors));
    }
    public static IResult NotImplemented(string? details = null) =>
        ErrorResponse(new Err(message: "Not implemented", Err.ErrCodes.NotImplemented, details, ErrorSource.Server));
    public static IResult FromErrOrNothing(ErrOrNothing possibleErr, Func<IResult> successFunc) =>
        possibleErr.IsErr(out var err) ? ErrorResponse(err) : successFunc();
    public static IResult FromErrListOrNothing(ErrListOrNothing possibleErrList, Func<IResult> successFunc) =>
        possibleErrList.IsErr(out var errs) ? ErrorResponse(errs) : successFunc();
    public static IResult FromErrOr<T>(ErrOr<T> errOrValue, Func<T, IResult> successFunc) =>
        errOrValue.Match(successFunc, ErrorResponse);
    public static IResult FromErrListOr<T>(ErrListOr<T> errListOrValue, Func<T, IResult> successFunc) =>
        errListOrValue.Match(successFunc, ErrorResponse);
    public static IResult Unauthorized() => Results.Unauthorized();
}