namespace SharedKernel.Common.errors;

public class ErrListOr<T>
{
    private readonly T _successValue;
    private readonly ErrList _errList;

    private ErrListOr(T value) {
        _successValue = value;
        _errList = new ErrList();
    }

    private ErrListOr(ErrList errList) {
        _errList = errList;
        _successValue = default!;
    }

    public static ErrListOr<T> Success(T value) => new ErrListOr<T>(value);

    public static ErrListOr<T> Errs(ErrList errList) => new ErrListOr<T>(errList);
    public bool AnyErr() => _errList.Any();
    public bool AnyErr(Func<Err, bool> predicate) => _errList.Any(predicate);

    public bool AnyErr(out ErrList errList) {
        if (_errList.Any()) {
            errList = _errList;
            return true;
        }

        errList = new ErrList();
        return false;
    }

    public bool IsSuccess() => !_errList.Any();

    public bool IsSuccess(out T success) {
        if (!AnyErr()) {
            success = _successValue;
            return true;
        }

        success = default!;
        return false;
    }

    public void ForEachErr(Action<Err> action) {
        _errList.ForEach(action);
    }

    public ErrList GetErrors() => _errList;

    public T GetSuccess() {
        if (IsSuccess()) {
            return _successValue;
        }
        throw new InvalidOperationException("No success value available.");
    }

    public void Match(Action<T> successAction, Action<ErrList> errorAction) {
        if (IsSuccess()) {
            successAction(_successValue);
        } else {
            errorAction(_errList);
        }
    }

    public static implicit operator ErrListOr<T>(ErrList errList) => new ErrListOr<T>(errList);
    public static implicit operator ErrListOr<T>(T successValue) => new ErrListOr<T>(successValue);
}
