namespace CryptoExchangeTask.Business.ExecutionPlan.Types;

public record ExecutionPlan
{
    public decimal TotalAmount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public IReadOnlyList<ExecutionPlanEntry> Orders { get; private set; } = [];

    public static ExecutionPlan Create(IReadOnlyCollection<ExecutionPlanEntry> orders)
    {
        return new ExecutionPlan
        {
            TotalPrice = orders.Select(entry => entry.Price * entry.Amount).Sum(),
            TotalAmount = orders.Sum(order => order.Amount),
            Orders = orders.ToList().AsReadOnly()
        };
    }
}