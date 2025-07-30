using System.Text.Json.Serialization;

namespace CryptoExchangeTask.Business.Types;

public record AvailableFunds
{
    [JsonPropertyName("Crypto")]
    public required decimal Crypto { get; set; }

    [JsonPropertyName("Euro")]
    public required decimal Euro { get; set; }
}