using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Repository.Types;

namespace CryptoExchangeTask.Business.ExecutionPlan.Calculators;

public interface IExecutionPlanCalculator
{
    IReadOnlyCollection<ExecutionPlanEntry> Calculate(decimal requestedAmount, IReadOnlyCollection<Exchange> exchanges);
}