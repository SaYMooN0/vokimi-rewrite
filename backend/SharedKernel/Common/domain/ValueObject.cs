namespace SharedKernel.Common;

public abstract class ValueObject
{
    public abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? other) {
        if (other is null || other.GetType() != GetType()) {
            return false;
        }

        return ((ValueObject)other)
                .GetEqualityComponents()
                .SequenceEqual(GetEqualityComponents());
    }

    public override int GetHashCode() {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
    public static bool operator ==(ValueObject? left, ValueObject? right) {
        if (left is not null) {
            return left.Equals(right);
        }
        return right is null;
    }

    public static bool operator !=(ValueObject? left, ValueObject? right) {
        return !(left == right);
    }
}