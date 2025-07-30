namespace CryptoExchangeTask.Business.Types;

public record Bid
{
    public required Order Order { get; set; }
}