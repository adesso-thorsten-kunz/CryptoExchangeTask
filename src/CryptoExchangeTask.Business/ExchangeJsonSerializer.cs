using System.Text.Json;
using System.Text.Json.Serialization;
using CryptoExchangeTask.Business.Types;

namespace CryptoExchangeTask.Business;

public class ExchangeJsonSerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public async Task<Exchange> FromFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await using var fileStream = File.OpenRead(filePath);
        // ToDo: Try...
        return await JsonSerializer.DeserializeAsync<Exchange>(fileStream, _jsonSerializerOptions, cancellationToken);
    }

    public string ToJson(Exchange value)
    {
        return JsonSerializer.Serialize(value, _jsonSerializerOptions);
    }
}