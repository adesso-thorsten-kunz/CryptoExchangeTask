namespace CryptoExchangeTask.Business.Repository.Types;

public record Ask
{
    public required Order Order { get; init; }
}