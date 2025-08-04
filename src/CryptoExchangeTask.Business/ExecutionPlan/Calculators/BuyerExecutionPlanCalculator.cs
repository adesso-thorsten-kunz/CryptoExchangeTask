using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Extensions;
using CryptoExchangeTask.Business.Repository.Types;

namespace CryptoExchangeTask.Business.ExecutionPlan.Calculators;

internal sealed class BuyerExecutionPlanCalculator : ExecutionPlanCalculatorBase
{
    protected override decimal GetAvailableFundsForCalculation(
        decimal availableEuro,
        decimal orderBookEntryPrice) => 
        availableEuro / orderBookEntryPrice;

    protected override decimal ReduceFundsBy(
        decimal executableOrderAmount, 
        decimal orderBookEntryPrice) => 
        executableOrderAmount * orderBookEntryPrice;

    protected override IReadOnlyCollection<OrderBookEntry> GetOrderBookEntries(
        IReadOnlyCollection<Exchange> exchanges) => 
        exchanges.GetAllAsksSortedByPrice();

    protected override Dictionary<string, decimal> GetAvailableFundsByExchange(
        IReadOnlyCollection<Exchange> exchanges) => 
        exchanges.ToAvailableEuroByExchangeId();
}