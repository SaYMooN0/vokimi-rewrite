using System.Text;

namespace SharedKernel.Common.errors;

public class ErrList
{
    private readonly List<Err> _errList;

    public ErrList() => _errList = new();

    public ErrList(Err err) => _errList = [err];

    public void Add(Err err) =>
        _errList.Add(err);
    public void AddPossibleErr<ErrOrT>(ErrOr<ErrOrT> possibleErr) {
        if (possibleErr.IsErr(out var err)) {
            _errList.Add(err);
        }
    }
    public void AddPossibleErr(ErrOrNothing possibleErr) {
        if (possibleErr.IsErr(out var err)) {
            _errList.Add(err);
        }
    }

    public void AddRange(IEnumerable<Err> errs) =>
        _errList.AddRange(errs);

    public void ForEach(Action<Err> action) {
        foreach (var err in _errList) {
            action(err);
        }
    }
    public bool Any() => _errList.Count > 0;
    public bool Any(Func<Err, bool> predicate) => _errList.Any(predicate);
    public bool All(Func<Err, bool> predicate) => _errList.All(predicate);

    public int Count() => _errList.Count;
    public Err? FirstOrNull() => Any() ? _errList[0] : null;
    public static implicit operator ErrList(Err err) {
        return new ErrList(err);
    }
    public Err[] ToArray() => _errList.ToArray();
    public override string ToString() {
        if (_errList.Count == 0) { return "No errors"; }
        if (_errList.Count == 1) { return _errList[0].ToString(); }

        var sb = new StringBuilder();
        for (int i = 0; i < _errList.Count; i++) {
            sb.AppendLine($"Error {i + 1}:\n{_errList[i].ToString()}");
        }
        return sb.ToString();
    }
}