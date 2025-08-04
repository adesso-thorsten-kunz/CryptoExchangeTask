using CryptoExchangeTask.Business.ExecutionPlan.Calculators;
using CryptoExchangeTask.Business.ExecutionPlan.Exceptions;
using CryptoExchangeTask.Business.Repository.Types;
using FluentAssertions;

namespace CryptoExchangeTask.Business.Tests;

public class SellerExecutionPlanCalculatorTests
{
    [Fact]
    public void Calculate_GivenNoExchangeFundsAndRequestOne_ThrowsNotEnoughFundsException()
    {
        // arrange
        var exchanges = new List<Exchange>
        {
            TestDataFactory.CreateExchange("exchange-01", 0, 0, [
                TestDataFactory.CreateAsk(Guid.NewGuid(), 1, 2000)
            ], [])
        };

        // act
        var sut = new SellerExecutionPlanCalculator();
        Action act = () => sut.Calculate(1, exchanges);

        // assert
        act.Should().Throw<NotEnoughFundsException>();
    }

    [Fact]
    public void Calculate_RequestNegativeOne_ThrowsArgumentOutOfRangeException()
    {
        // arrange
        var exchanges = new List<Exchange>
        {
            TestDataFactory.CreateExchange("exchange-01", 0, 0, [
                TestDataFactory.CreateAsk(Guid.NewGuid(), 1, 2000)
            ], [])
        };

        // act
        var sut = new SellerExecutionPlanCalculator();
        Action act = () => sut.Calculate(-1, exchanges);

        // assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Calculate_GivenEnoughFundsInOneExchangeRequestOne_ReturnsMatchingOneEntry()
    {
        // arrange
        var askOneId = Guid.NewGuid();
        var exchanges = new List<Exchange>
        {
            TestDataFactory.CreateExchange("exchange-01", 1, 0, [], [
                TestDataFactory.CreateBid(Guid.NewGuid(), 0.5m, 1000),
                TestDataFactory.CreateBid(Guid.NewGuid(), 1, 2000),
                TestDataFactory.CreateBid(askOneId, 1, 3000),
                TestDataFactory.CreateBid(Guid.NewGuid(), 0.5m, 2100)
            ])
        };

        // act
        var sut = new SellerExecutionPlanCalculator();
        var executionPlanEntries = sut.Calculate(1, exchanges);

        // assert
        executionPlanEntries.Count.Should().Be(1);
        executionPlanEntries.Should().SatisfyRespectively(
            first =>
            {
                first.OrderId.Should().Be(askOneId);
            });
    }

    [Fact]
    public void Calculate_GivenEnoughFundsInTwoExchangeRequestTwo_ReturnsMatchingFourEntries()
    {
        // arrange
        var askOneId = Guid.NewGuid();
        var askTwoId = Guid.NewGuid();
        var askThreeId = Guid.NewGuid();
        var askFourId = Guid.NewGuid();
        const string exchangeOne = "exchange-01";
        const string exchangeTwo = "exchange-02";
        var exchanges = new List<Exchange>
        {
            TestDataFactory.CreateExchange(exchangeOne, 1, 0, [], [
                TestDataFactory.CreateBid(askTwoId, 0.5m, 4000),
                TestDataFactory.CreateBid(Guid.NewGuid(), 1, 2000),
                TestDataFactory.CreateBid(askThreeId, 1, 3000),
                TestDataFactory.CreateBid(Guid.NewGuid(), 0.5m, 2100)
            ]),
            TestDataFactory.CreateExchange(exchangeTwo, 1, 0, [], [
                TestDataFactory.CreateBid(askOneId, 0.5m, 4500),
                TestDataFactory.CreateBid(Guid.NewGuid(), 1, 2000),
                TestDataFactory.CreateBid(askFourId, 1, 2800),
                TestDataFactory.CreateBid(Guid.NewGuid(), 0.5m, 2100)
            ])
        };

        // act
        var sut = new SellerExecutionPlanCalculator();
        var executionPlanEntries = sut.Calculate(2, exchanges);

        // assert
        executionPlanEntries.Count.Should().Be(4);
        executionPlanEntries.Should().SatisfyRespectively(
            first =>
            {
                first.ExchangeId.Should().Be(exchangeTwo);
                first.OrderId.Should().Be(askOneId);
                first.Amount.Should().Be(0.5m);
                first.Price.Should().Be(4500);
            },
            second =>
            {
                second.ExchangeId.Should().Be(exchangeOne);
                second.OrderId.Should().Be(askTwoId);
                second.Amount.Should().Be(0.5m);
                second.Price.Should().Be(4000);
            },
            third =>
            {
                third.ExchangeId.Should().Be(exchangeOne);
                third.OrderId.Should().Be(askThreeId);
                third.Amount.Should().Be(0.5m);
                third.Price.Should().Be(3000);
            },
            fourth =>
            {
                fourth.ExchangeId.Should().Be(exchangeTwo);
                fourth.OrderId.Should().Be(askFourId);
                fourth.Amount.Should().Be(0.5m);
                fourth.Price.Should().Be(2800);
            });
    }
}