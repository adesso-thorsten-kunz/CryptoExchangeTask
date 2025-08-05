namespace CryptoExchangeTask.API.Contracts.ExecutionPlan;

public record ExecutionPlanResponse
{
    public decimal TotalAmount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public int TotalOrders { get; private set; }
    public IReadOnlyList<OrderBookEntry> Orders { get; private set; } = [];

    public static ExecutionPlanResponse Create(IReadOnlyCollection<OrderBookEntry> orders)
    {
        return new ExecutionPlanResponse
        {
            TotalPrice = orders.Select(entry => entry.Price * entry.Amount).Sum(),
            TotalAmount = orders.Sum(order => order.Amount),
            TotalOrders = orders.Count,
            Orders = orders.ToList().AsReadOnly()
        };
    }
}

public record OrderBookEntry
{
    public required string ExchangeId { get; init; }
    public required Guid OrderId { get; init; }
    public required DateTime Time { get; init; }
    public required OrderType Type { get; init; }
    public required OrderKind Kind { get; init; }
    public required decimal Amount { get; init; }
    public required decimal Price { get; init; }
}

public enum OrderKind
{
    Limit
}