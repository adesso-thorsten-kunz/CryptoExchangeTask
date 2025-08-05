using CryptoExchangeTask.Business.ExecutionPlan.Calculators;
using CryptoExchangeTask.Business.ExecutionPlan.Exceptions;
using CryptoExchangeTask.Business.Repository.Types;
using FluentAssertions;

namespace CryptoExchangeTask.Business.Tests;

public class BuyerExecutionPlanCalculatorTests
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
        var sut = new BuyerExecutionPlanCalculator();
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
        var sut = new BuyerExecutionPlanCalculator();
        Action act = () => sut.Calculate(-1, exchanges);

        // assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Calculate_GivenEnoughFundsRequestOne_ReturnsOneEntry()
    {
        // arrange
        var exchanges = new List<Exchange>
        {
            TestDataFactory.CreateExchange("exchange-01", 0, 2000, [
                TestDataFactory.CreateAsk(Guid.NewGuid(), 1, 2000)
            ], [])
        };

        // act
        var sut = new BuyerExecutionPlanCalculator();
        var executionPlanEntries = sut.Calculate(1, exchanges);

        // assert
        executionPlanEntries.Count.Should().Be(1);
    }

    [Fact]
    public void Calculate_GivenEnoughFundsInOneExchangeRequestTwo_ReturnsMatchingThreeEntry()
    {
        // arrange
        var askOneId = Guid.NewGuid();
        var askTwoId = Guid.NewGuid();
        var askThreeId = Guid.NewGuid();
        var exchanges = new List<Exchange>
        {
            TestDataFactory.CreateExchange("exchange-01", 0, 4000, [
                TestDataFactory.CreateAsk(askOneId, 0.5m, 1000),
                TestDataFactory.CreateAsk(askTwoId, 1, 2000),
                TestDataFactory.CreateAsk(Guid.NewGuid(), 1, 3000),
                TestDataFactory.CreateAsk(askThreeId, 0.5m, 2100)
            ], [])
        };

        // act
        var sut = new BuyerExecutionPlanCalculator();
        var executionPlanEntries = sut.Calculate(2, exchanges);

        // assert
        executionPlanEntries.Count.Should().Be(3);
        executionPlanEntries.Should().SatisfyRespectively(
            first =>
            {
                first.OrderId.Should().Be(askOneId);
            },
            second =>
            {
                second.OrderId.Should().Be(askTwoId);
            },
            third =>
            {
                third.OrderId.Should().Be(askThreeId);
            });
    }

    [Fact]
    public void Calculate_GivenEnoughFundsInTwoExchangesRequestTwo_ReturnsMatchingFourEntries()
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
            TestDataFactory.CreateExchange(exchangeOne, 0, 4000, [
                TestDataFactory.CreateAsk(askOneId, 0.5m, 1600),
                TestDataFactory.CreateAsk(Guid.NewGuid(), 1, 2100),
                TestDataFactory.CreateAsk(askTwoId, 0.5m, 1500)
            ], []),
            TestDataFactory.CreateExchange(exchangeTwo, 0, 4000, [
                TestDataFactory.CreateAsk(Guid.NewGuid(), 0.5m, 4000),
                TestDataFactory.CreateAsk(Guid.NewGuid(), 1, 3000),
                TestDataFactory.CreateAsk(askThreeId, 1, 2000),
                TestDataFactory.CreateAsk(askFourId, 0.5m, 1000)
            ], [])
        };

        // act
        var sut = new BuyerExecutionPlanCalculator();
        var executionPlanEntries = sut.Calculate(2, exchanges);

        // assert
        executionPlanEntries.Count.Should().Be(4);
        executionPlanEntries.Should().SatisfyRespectively(
            first =>
            {
                first.OrderId.Should().Be(askFourId);
                first.ExchangeId.Should().Be(exchangeTwo);
            },
            second =>
            {
                second.OrderId.Should().Be(askTwoId);
                second.ExchangeId.Should().Be(exchangeOne);
            },
            third =>
            {
                third.OrderId.Should().Be(askOneId);
                third.ExchangeId.Should().Be(exchangeOne);
            },
            fourth =>
            {
                fourth.OrderId.Should().Be(askThreeId);
                fourth.ExchangeId.Should().Be(exchangeTwo);
            });
    }
}