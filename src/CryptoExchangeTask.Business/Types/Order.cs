using System.Text.Json.Serialization;

namespace CryptoExchangeTask.Business.Types;

public record Order
{
    [JsonPropertyName("Id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("Time")]
    public required DateTime Time { get; set; }

    [JsonPropertyName("Type")]
    public required OrderType Type { get; set; }

    [JsonPropertyName("Kind")]
    public required OrderKind Kind { get; set; }

    [JsonPropertyName("Amount")]
    public required decimal Amount { get; set; }

    [JsonPropertyName("Price")]
    public required decimal Price { get; set; }
}