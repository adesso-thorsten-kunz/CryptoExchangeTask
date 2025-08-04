using CryptoExchangeTask.Business.Repository.Types;
using System.Reflection;

namespace CryptoExchangeTask.Business.Repository;

public interface IExchangeRepository
{
    Task<IReadOnlyCollection<Exchange>> FetchAllExchangesAsync();
}

internal sealed class JsonExchangeRepository : IExchangeRepository
{
    private readonly IExchangeJsonSerializer _serializer;

    public JsonExchangeRepository(IExchangeJsonSerializer serializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task<IReadOnlyCollection<Exchange>> FetchAllExchangesAsync()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePaths = Directory.GetFiles(Path.Combine(assemblyPath, "Repository", "Data"), "exchange-*.json");

        var exchanges = new List<Exchange>(filePaths.Length);
        foreach (var filePath in filePaths)
        {
            var exchange = await _serializer.FromFileAsync(filePath);
            exchanges.Add(exchange);
        }
     
        return exchanges;
    }
}