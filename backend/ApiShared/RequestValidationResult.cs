using OneOf;
using OneOf.Types;
using SharedKernel.Common.errors;
namespace ApiShared;

public class RequestValidationResult
{
    OneOf<Success, Err, ErrList> result;
    public bool AnyErrors() => result.IsT1 || result.IsT2;
    public bool AnyErrors(out Err[] errors) {
        if (result.IsT1) {
            errors = [result.AsT1];
            return true;
        }
        if (result.IsT2) {
            errors = result.AsT2.ToArray();
            return true;
        }
        errors = [];
        return false;
    }

    private RequestValidationResult(OneOf<Success, Err, ErrList> result) => this.result = result;


    public static implicit operator RequestValidationResult(Err err) =>
        new RequestValidationResult(err);

    public static implicit operator RequestValidationResult(ErrList errList) =>
        new RequestValidationResult(errList);

    public readonly static RequestValidationResult Success = new(new Success());

}
