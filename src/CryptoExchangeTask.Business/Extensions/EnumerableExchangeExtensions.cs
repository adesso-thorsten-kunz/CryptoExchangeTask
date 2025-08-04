using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Repository.Types;

namespace CryptoExchangeTask.Business.Extensions;

internal static class EnumerableExchangeExtensions
{
    public static Dictionary<string, decimal> ToAvailableEuroByExchangeId(this IEnumerable<Exchange> exchanges) => 
        exchanges.ToDictionary(
            exchange => exchange.Id, 
            exchange => exchange.AvailableFunds.Euro);

    public static Dictionary<string, decimal> ToAvailableCryptoByExchangeId(this IEnumerable<Exchange> exchanges) =>
        exchanges.ToDictionary(
            exchange => exchange.Id,
            exchange => exchange.AvailableFunds.Crypto);

    public static IReadOnlyCollection<OrderBookEntry> GetAllBidsSortedByPrice(this IEnumerable<Exchange> exchanges) =>
        exchanges
            .SelectMany(exchange => exchange.OrderBook.Bids
                .Select(ask => new OrderBookEntry
                {
                    ExchangeId = exchange.Id,
                    OrderId = ask.Order.Id,
                    Amount = ask.Order.Amount,
                    Price = ask.Order.Price
                }))
            .OrderByDescending(candidate => candidate.Price)
            .ToList()
            .AsReadOnly();

    public static IReadOnlyCollection<OrderBookEntry> GetAllAsksSortedByPrice(this IEnumerable<Exchange> exchanges) =>
        exchanges
            .SelectMany(exchange => exchange.OrderBook.Asks
                .Select(ask => new OrderBookEntry
                {
                    ExchangeId = exchange.Id,
                    OrderId = ask.Order.Id,
                    Amount = ask.Order.Amount,
                    Price = ask.Order.Price
                }))
            .OrderBy(candidate => candidate.Price)
            .ToList()
            .AsReadOnly();
}