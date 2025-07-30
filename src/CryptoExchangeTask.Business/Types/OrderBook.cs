using System.Text.Json.Serialization;

namespace CryptoExchangeTask.Business.Types;

public record OrderBook
{
    [JsonPropertyName("Bids")]
    public required IReadOnlyCollection<Bid> Bids { get; set; }

    [JsonPropertyName("Asks")]
    public required IReadOnlyCollection<Ask> Asks { get; set; }
}