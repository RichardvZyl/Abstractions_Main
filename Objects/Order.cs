namespace Abstractions.Objects;

public class Order
{
    public Order()
    {
        Ascending = true;
        Property ??= string.Empty;
    }

    public bool Ascending { get; set; }

    public string Property { get; set; }
}
