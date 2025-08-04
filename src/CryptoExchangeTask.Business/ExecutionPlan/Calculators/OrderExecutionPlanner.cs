namespace CryptoExchangeTask.Business.ExecutionPlan.Calculators;

internal static class OrderExecutionPlanner
{
    public static decimal CalculateExecutableOrderAmount(
        decimal requestedAmount,
        decimal alreadyPlannedAmount,
        decimal availableFunds,
        decimal availableOrderBookAmount)
    {
        var remainingRequestedAmount = requestedAmount - alreadyPlannedAmount;
        return Math.Min(availableOrderBookAmount, 
            Math.Min(remainingRequestedAmount , availableFunds));
    }
}