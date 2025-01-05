using OneOf;

namespace SharedKernel.Common.errors;

public class ErrOr<T>
{
    private readonly OneOf<T, Err> value;

    private ErrOr(T value) => this.value = OneOf<T, Err>.FromT0(value);
    private ErrOr(Err err) => this.value = OneOf<T, Err>.FromT1(err);
    public static ErrOr<T> Success(T value) => new ErrOr<T>(value);
    public static ErrOr<T> Error(Err err) => new ErrOr<T>(err);

    public TResult Match<TResult>(Func<T, TResult> successAction, Func<Err, TResult> errorAction) =>
        value.Match(successAction, errorAction);
    public bool IsErr() => value.IsT1;
    public bool IsErr(out Err err) {
        if (value.IsT1) {
            err = value.AsT1;
            return true;
        }

        err = new Err("No error");
        return false;
    }
    public bool IsSuccess() => value.IsT0;

    public bool IsSuccess(out T success) {
        if (value.IsT0) {
            success = value.AsT0;
            return true;
        }

        success = default!;
        return false;
    }

    public static implicit operator ErrOr<T>(Err err) => new ErrOr<T>(err);

    public static implicit operator ErrOr<T>(T value) => new ErrOr<T>(value);

    public Err GetErr() => value.IsT1 ? value.AsT1 : new Err("No error");
    public T GetValue() {
        if (value.IsT0) {
            return value.AsT0;
        }
        throw new InvalidOperationException("No success value available.");
    }
}
