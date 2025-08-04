using CryptoExchangeTask.Business.ExecutionPlan.Calculators;
using CryptoExchangeTask.Business.Repository;
using CryptoExchangeTask.Business.Repository.Types;

namespace CryptoExchangeTask.Business.ExecutionPlan;

public interface IExecutionPlanService
{
    Task<Types.ExecutionPlan> Create(
        decimal requestedAmount,
        OrderType orderType);
}

internal sealed class ExecutionPlanService : IExecutionPlanService
{
    private readonly IExchangeRepository _exchangeRepository;
    private readonly IExecutionPlanCalculatorFactory _executionPlanCalculatorFactory;

    public ExecutionPlanService(
        IExchangeRepository exchangeRepository, 
        IExecutionPlanCalculatorFactory executionPlanCalculatorFactory)
    {
        _exchangeRepository = exchangeRepository ?? throw new ArgumentNullException(nameof(exchangeRepository));
        _executionPlanCalculatorFactory = executionPlanCalculatorFactory ?? throw new ArgumentNullException(nameof(executionPlanCalculatorFactory));
    }

    public async Task<Types.ExecutionPlan> Create(
        decimal requestedAmount,
        OrderType orderType)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(requestedAmount, nameof(requestedAmount));

        var exchanges = await _exchangeRepository.FetchAllExchangesAsync();

        var executionPlanCalculator = _executionPlanCalculatorFactory.Create(orderType);

        var executionPlanEntries = executionPlanCalculator.Calculate(requestedAmount, exchanges);
        
        return Types.ExecutionPlan.Create(executionPlanEntries);
    }
}