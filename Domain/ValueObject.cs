namespace Abstractions.Domain;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator !=(ValueObject a, ValueObject b)
        => !(a == b);

    public static bool operator ==(ValueObject a, ValueObject b)
        => a is null && b is null || a is not null && b is not null && a.Equals(b);

    public override bool Equals(object? obj)
        => obj is not null
            && (ReferenceEquals(this, obj) || GetType() == obj.GetType() && GetEquals().SequenceEqual(((ValueObject)obj).GetEquals()));

    public override int GetHashCode()
        => GetEquals().Aggregate(0, (a, b) => (a * 97) + b.GetHashCode());

    protected abstract IEnumerable<object> GetEquals();

    public bool Equals(ValueObject? other)
        => other is not null
            && (ReferenceEquals(this, other) || GetType() == other.GetType());
}
