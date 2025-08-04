namespace CryptoExchangeTask.Business.Repository.Types;

public record Bid
{
    public required Order Order { get; init; }
}