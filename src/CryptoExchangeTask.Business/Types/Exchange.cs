using System.Text.Json.Serialization;

namespace CryptoExchangeTask.Business.Types;

public record Exchange
{
    [JsonPropertyName("Id")]
    public required string Id { get; set; }

    [JsonPropertyName("AvailableFunds")]
    public required AvailableFunds AvailableFunds { get; set; }

    [JsonPropertyName("OrderBook")]
    public required OrderBook OrderBook { get; set; }
}