namespace CryptoExchangeTask.API.Contracts.ExecutionPlan;

public record ExecutionPlanRequest
{
    public required decimal Amount { get; set; }

    public required OrderType OrderType { get; set; }
}

public enum OrderType
{
    Buy,
    Sell
}