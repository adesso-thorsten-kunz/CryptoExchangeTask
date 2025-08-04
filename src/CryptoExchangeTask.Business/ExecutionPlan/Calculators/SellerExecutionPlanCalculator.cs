using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Extensions;
using CryptoExchangeTask.Business.Repository.Types;

namespace CryptoExchangeTask.Business.ExecutionPlan.Calculators;

internal sealed class SellerExecutionPlanCalculator : ExecutionPlanCalculatorBase
{
    protected override decimal GetAvailableFundsForCalculation(
        decimal availableCrypto,
        decimal orderBookEntryPrice) =>
        availableCrypto;

    protected override decimal ReduceFundsBy(
        decimal executableOrderAmount, 
        decimal orderBookEntryPrice) => 
        executableOrderAmount;

    protected override IReadOnlyCollection<OrderBookEntry> GetOrderBookEntries(
        IReadOnlyCollection<Exchange> exchanges) => 
        exchanges.GetAllBidsSortedByPrice();

    protected override Dictionary<string, decimal> GetAvailableFundsByExchange(
        IReadOnlyCollection<Exchange> exchanges) => 
        exchanges.ToAvailableCryptoByExchangeId();
}