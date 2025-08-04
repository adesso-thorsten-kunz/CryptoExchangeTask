namespace CryptoExchangeTask.Business.ExecutionPlan.Types;

public record OrderBookEntry
{
    public required string ExchangeId { get; init; }
    public required Guid OrderId { get; init; }
    public required decimal Amount { get; init; }
    public required decimal Price { get; init; }
}