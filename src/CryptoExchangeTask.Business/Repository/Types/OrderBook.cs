using System.Text.Json.Serialization;

namespace CryptoExchangeTask.Business.Repository.Types;

public record OrderBook
{
    [JsonPropertyName("Bids")]
    public required IReadOnlyCollection<Bid> Bids { get; init; }

    [JsonPropertyName("Asks")]
    public required IReadOnlyCollection<Ask> Asks { get; init; }
}