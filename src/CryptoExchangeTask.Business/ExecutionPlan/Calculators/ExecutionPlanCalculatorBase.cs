using CryptoExchangeTask.Business.ExecutionPlan.Exceptions;
using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Repository.Types;

namespace CryptoExchangeTask.Business.ExecutionPlan.Calculators;

internal abstract class ExecutionPlanCalculatorBase : IExecutionPlanCalculator
{
    public IReadOnlyCollection<OrderBookEntry> Calculate(
        decimal requestedAmount,
        IReadOnlyCollection<Exchange> exchanges)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(requestedAmount, nameof(requestedAmount));

        var availableFundsByExchange = GetAvailableFundsByExchange(exchanges);

        var orderBookEntries = GetOrderBookEntries(exchanges);

        return Calculate(requestedAmount, availableFundsByExchange, orderBookEntries);
    }

    protected abstract decimal GetAvailableFundsForCalculation(decimal availableFunds, decimal orderBookEntryPrice);

    protected abstract decimal ReduceFundsBy(decimal executableOrderAmount, decimal orderBookEntryPrice);

    protected abstract IReadOnlyCollection<OrderBookEntry> GetOrderBookEntries(IReadOnlyCollection<Exchange> exchanges);

    protected abstract Dictionary<string, decimal> GetAvailableFundsByExchange(IReadOnlyCollection<Exchange> exchanges);

    private IReadOnlyCollection<OrderBookEntry> Calculate(
        decimal requestedAmount,
        Dictionary<string, decimal> availableFundsByExchangeId,
        IReadOnlyCollection<OrderBookEntry> orderBookEntries)
    {
        decimal alreadyPlannedAmount = 0;

        List<OrderBookEntry> executionPlanEntries = [];

        foreach (var orderBookEntry in orderBookEntries)
        {
            if (IsRequestedAmountFulfilled(alreadyPlannedAmount, requestedAmount))
            {
                break;
            }

            var exchangeAvailableFunds = availableFundsByExchangeId[orderBookEntry.ExchangeId];
            if (!HasFunds(exchangeAvailableFunds))
            {
                continue;
            }

            var executableOrderAmount = OrderExecutionPlanner.CalculateExecutableOrderAmount(
                requestedAmount,
                alreadyPlannedAmount,
                GetAvailableFundsForCalculation(exchangeAvailableFunds, orderBookEntry.Price),
                orderBookEntry.Amount);

            if (!IsOrderBookEntryExecutable(executableOrderAmount))
            {
                continue;
            }

            executionPlanEntries.Add(orderBookEntry with { Amount = executableOrderAmount });

            alreadyPlannedAmount += executableOrderAmount;
            availableFundsByExchangeId[orderBookEntry.ExchangeId] -=
                ReduceFundsBy(executableOrderAmount, orderBookEntry.Price);
        }

        ThrowIfRequestedAmountNotFulfilled(alreadyPlannedAmount, requestedAmount);

        return executionPlanEntries
            .AsReadOnly();
    }

    private static bool IsRequestedAmountFulfilled(
        decimal alreadyPlannedAmount,
        decimal requestedAmount) =>
        alreadyPlannedAmount >= requestedAmount;

    private static bool HasFunds(decimal availableFunds) =>
        availableFunds > 0;

    private static bool IsOrderBookEntryExecutable(decimal executableOrderAmount) =>
        executableOrderAmount > 0;

    private static void ThrowIfRequestedAmountNotFulfilled(
        decimal alreadyPlannedAmount,
        decimal requestedAmount)
    {
        if (alreadyPlannedAmount < requestedAmount)
        {
            throw new NotEnoughFundsException();
        }
    }
}