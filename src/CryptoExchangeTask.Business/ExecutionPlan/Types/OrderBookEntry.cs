using CryptoExchangeTask.Business.Repository.Types;

namespace CryptoExchangeTask.Business.ExecutionPlan.Types;

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