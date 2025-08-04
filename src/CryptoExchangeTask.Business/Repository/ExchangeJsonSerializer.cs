using System.Text.Json;
using System.Text.Json.Serialization;
using CryptoExchangeTask.Business.Repository.Types;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeTask.Business.Repository;

public interface IExchangeJsonSerializer
{
    Task<Exchange> FromFileAsync(string filePath, CancellationToken cancellationToken = default);
}

internal sealed class ExchangeJsonSerializer : IExchangeJsonSerializer
{
    private readonly ILogger<ExchangeJsonSerializer> _logger;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public ExchangeJsonSerializer(ILogger<ExchangeJsonSerializer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Exchange> FromFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File '{filePath}' does not exist");
        }

        await using var fileStream = File.OpenRead(filePath);
        
        try
        {
            return await JsonSerializer.DeserializeAsync<Exchange>(
                fileStream, 
                _jsonSerializerOptions,
                cancellationToken) ?? throw new InvalidOperationException("Deserialization returned null");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exchange deserialization failed");
            throw;
        }
    }
}