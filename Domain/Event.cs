namespace Abstractions.Domain;

public abstract class Event
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTimeOffset DateTime { get; } = DateTimeOffset.UtcNow;
}
