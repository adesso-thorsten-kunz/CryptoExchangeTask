using System.Text.Json.Serialization;

namespace CryptoExchangeTask.Business.Repository.Types;

public record Exchange
{
    [JsonPropertyName("Id")]
    public required string Id { get; init; }

    [JsonPropertyName("AvailableFunds")]
    public required AvailableFunds AvailableFunds { get; init; }

    [JsonPropertyName("OrderBook")]
    public required OrderBook OrderBook { get; init; }
}