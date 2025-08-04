using CryptoExchangeTask.Business.Extensions;
using CryptoExchangeTask.Business.Repository.Types;
using FluentAssertions;

namespace CryptoExchangeTask.Business.Tests;

public class EnumerableExchangeExtensionsTests
{
    [Fact]
    public void ToAvailableEuroByExchangeId_GivenTwoExchanges_ReturnsExpectedDictionary()
    {
        // arrange
        const string exchangeOneId = "exchange-05";
        const int exchangeOneEuro = 2;

        const string exchangeTwoId = "exchange-02";
        const int exchangeTwoEuro = 10;

        IEnumerable<Exchange> exchanges =
        [
            TestDataFactory.CreateExchange(exchangeOneId, 0, exchangeOneEuro, [], []),
            TestDataFactory.CreateExchange(exchangeTwoId, 0, exchangeTwoEuro, [], [])
        ];

        // act
        var availableEuroByExchangeId = exchanges.ToAvailableEuroByExchangeId();

        // assert
        availableEuroByExchangeId.Count.Should().Be(2);
        availableEuroByExchangeId[exchangeOneId].Should().Be(exchangeOneEuro);
        availableEuroByExchangeId[exchangeTwoId].Should().Be(exchangeTwoEuro);
    }

    [Fact]
    public void ToAvailableCryptoByExchangeId_GivenThreeExchanges_ReturnsExpectedDictionary()
    {
        // arrange
        const string exchangeOneId = "exchange-05";
        const int exchangeOneCrypto = 20;

        const string exchangeTwoId = "exchange-02";
        const int exchangeTwoCrypto = 10;

        const string exchangeThreeId = "exchange-10";
        const int exchangeThreeCrypto = 100;

        IEnumerable<Exchange> exchanges =
        [
            TestDataFactory.CreateExchange(exchangeOneId, exchangeOneCrypto, 0, [], []),
            TestDataFactory.CreateExchange(exchangeTwoId, exchangeTwoCrypto, 0, [], []),
            TestDataFactory.CreateExchange(exchangeThreeId, exchangeThreeCrypto, 0, [], [])
        ];

        // act
        var availableCryptoByExchangeId = exchanges.ToAvailableCryptoByExchangeId();

        // assert
        availableCryptoByExchangeId.Count.Should().Be(3);
        availableCryptoByExchangeId[exchangeOneId].Should().Be(exchangeOneCrypto);
        availableCryptoByExchangeId[exchangeTwoId].Should().Be(exchangeTwoCrypto);
        availableCryptoByExchangeId[exchangeThreeId].Should().Be(exchangeThreeCrypto);
    }

    [Fact]
    public void GetAllBidsSortedByPrice_GivenAListOfExchanges_ReturnsExpectedOrderedBids()
    {
        // arrange
        var exchanges = CreateExchanges();

        // act
        var allBidsSortedByPrice = exchanges.GetAllBidsSortedByPrice();

        // assert
        Assert.Equal(6, allBidsSortedByPrice.Count);
        allBidsSortedByPrice.Should().BeInDescendingOrder(x => x.Price);
    }

    [Fact]
    public void GetAllAsksSortedByPrice_GivenAListOfExchanges_ReturnsExpectedOrderedAsks()
    {
        // arrange
        var exchanges = CreateExchanges();

        // act
        var allAsksSortedByPrice = exchanges.GetAllAsksSortedByPrice();

        // assert
        Assert.Equal(4, allAsksSortedByPrice.Count);
        allAsksSortedByPrice.Should().BeInAscendingOrder(x => x.Price);
    }

    private static IEnumerable<Exchange> CreateExchanges() =>
        [
            TestDataFactory.CreateExchange("1", 0, 0, [
                    TestDataFactory.CreateAsk(Guid.NewGuid(), 0, 1),
                    TestDataFactory.CreateAsk(Guid.NewGuid(), 0, 5)
                ],
                [
                    TestDataFactory.CreateBid(Guid.NewGuid(), 0, 10),
                    TestDataFactory.CreateBid(Guid.NewGuid(), 0, 50),
                    TestDataFactory.CreateBid(Guid.NewGuid(), 0, 80),
                    TestDataFactory.CreateBid(Guid.NewGuid(), 0, 90)
                ]),
            TestDataFactory.CreateExchange("2", 0, 0, [
                    TestDataFactory.CreateAsk(Guid.NewGuid(), 0, 4),
                    TestDataFactory.CreateAsk(Guid.NewGuid(), 0, 6),
                ],
                [
                    TestDataFactory.CreateBid(Guid.NewGuid(), 0, 40),
                    TestDataFactory.CreateBid(Guid.NewGuid(), 0, 30)
                ]),
        ];
}