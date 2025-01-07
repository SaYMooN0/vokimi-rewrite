namespace SharedKernel.Common.errors;

public class ErrListOrNothing
{
    private ErrList _errors;

    private ErrListOrNothing(ErrList errs) { _errors = errs; }

    public bool IsErr() => _errors.Any();

    public bool IsErr(out ErrList errs) {
        if (_errors.Any()) {
            errs = _errors;
            return true;
        }

        errs = new();
        return false;
    }

    public static implicit operator ErrListOrNothing(ErrList errs) => new ErrListOrNothing(errs);
    public static implicit operator ErrListOrNothing(Err singleErr) => new ErrListOrNothing(new(singleErr));

    public static readonly ErrListOrNothing Nothing = new ErrListOrNothing(new());
}
