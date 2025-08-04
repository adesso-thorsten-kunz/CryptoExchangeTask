namespace CryptoExchangeTask.API.Contracts;

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