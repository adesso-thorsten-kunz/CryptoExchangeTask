using System.Text.Json.Serialization;

namespace CryptoExchangeTask.Business.Repository.Types;

public record AvailableFunds
{
    [JsonPropertyName("Crypto")]
    public required decimal Crypto { get; init; }

    [JsonPropertyName("Euro")]
    public required decimal Euro { get; init; }
}