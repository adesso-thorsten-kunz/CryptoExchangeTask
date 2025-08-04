using System.Text.Json.Serialization;

namespace CryptoExchangeTask.Business.Repository.Types;

public record Order
{
    [JsonPropertyName("Id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("Time")]
    public required DateTime Time { get; init; }

    [JsonPropertyName("Type")]
    public required OrderType Type { get; init; }

    [JsonPropertyName("Kind")]
    public required OrderKind Kind { get; init; }

    [JsonPropertyName("Amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("Price")]
    public required decimal Price { get; init; }
}