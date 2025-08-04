using CryptoExchangeTask.Business.Repository.Types;

namespace CryptoExchangeTask.Business.Tests;

public static class TestDataFactory
{
    public static Exchange CreateExchange(
        string id,
        decimal crypto,
        decimal euro,
        IEnumerable<Ask> asks,
        IEnumerable<Bid> bids) =>
        new()
        {
            Id = id,
            AvailableFunds = new AvailableFunds
            {
                Crypto = crypto,
                Euro = euro
            },
            OrderBook = new OrderBook
            {
                Asks = asks.ToList().AsReadOnly(),
                Bids = bids.ToList().AsReadOnly()
            }
        };

    public static Bid CreateBid(
        Guid id,
        decimal amount,
        decimal price) =>
        new()
        {
            Order = CreateOrder(id, amount, price, OrderType.Buy)
        };


    public static Ask CreateAsk(
        Guid id,
        decimal amount,
        decimal price) =>
        new()
        {
            Order = CreateOrder(id, amount, price, OrderType.Sell)
        };


    public static Order CreateOrder(Guid id,
        decimal amount,
        decimal price,
        OrderType orderType) =>
        new()
        {
            Id = id,
            Amount = amount,
            Kind = OrderKind.Limit,
            Price = price,
            Time = DateTime.Now,
            Type = orderType
        };
}