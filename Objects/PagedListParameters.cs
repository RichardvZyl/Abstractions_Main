namespace Abstractions.Objects;

public class PagedListParameters
{
    public Order Order { get; set; } = new Order();

    public Page Page { get; set; } = new Page();
}
