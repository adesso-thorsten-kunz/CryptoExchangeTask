namespace CryptoExchangeTask.Business.Types;

public record Ask
{
    public required Order Order { get; set; }
}