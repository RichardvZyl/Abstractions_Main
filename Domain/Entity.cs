namespace Abstractions.Domain;

public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : struct
{
    protected Entity()
        => Id = default;

    protected Entity(TId id)
        => Id = id;

    public virtual TId Id { get; init; }

    public bool IsDeleted { get; set; } = false;

    public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.UtcNow;


    public static bool operator !=(Entity<TId> a, Entity<TId> b)
        => !(a == b);

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
        => (a is null && b is null) || (a is not null && b is not null && a.Equals(b));

    public override bool Equals(object? obj)
        => obj is null ? base.Equals(obj) : Equals((Entity<TId>)obj);

    public bool Equals(Entity<TId>? other)
        => other is not null
            && (ReferenceEquals(this, other) || (GetType() == other.GetType() && Id.Equals(other.Id)));

    public override int GetHashCode()
    {
        unchecked
        {
            return (GetType().GetHashCode() * 97) ^ Id.GetHashCode();
        }
    }
}
