namespace CryptoExchangeTask.Business.ExecutionPlan.Types;

public record ExecutionPlan
{
    public decimal TotalAmount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public int TotalOrders { get; private set; }
    public IReadOnlyList<ExecutionPlanEntry> Orders { get; private set; } = [];

    public static ExecutionPlan Create(IReadOnlyCollection<ExecutionPlanEntry> orders)
    {
        return new ExecutionPlan
        {
            TotalPrice = orders.Select(entry => entry.Price * entry.Amount).Sum(),
            TotalAmount = orders.Sum(order => order.Amount),
            TotalOrders = orders.Count,
            Orders = orders.ToList().AsReadOnly()
        };
    }
}
