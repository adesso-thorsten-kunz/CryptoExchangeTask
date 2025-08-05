using CryptoExchangeTask.Business.ExecutionPlan.Calculators;
using CryptoExchangeTask.Business.ExecutionPlan.Exceptions;
using CryptoExchangeTask.Business.Repository;
using CryptoExchangeTask.Business.Repository.Types;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeTask.Business.ExecutionPlan;

public interface IExecutionPlanService
{
    Task<Types.ExecutionPlan> CreateAsync(
        decimal requestedAmount,
        OrderType orderType);
}

internal sealed class ExecutionPlanService : IExecutionPlanService
{
    private readonly IExchangeRepository _exchangeRepository;
    private readonly IExecutionPlanCalculatorFactory _executionPlanCalculatorFactory;
    private readonly ILogger<ExecutionPlanService> _logger;

    public ExecutionPlanService(
        IExchangeRepository exchangeRepository, 
        IExecutionPlanCalculatorFactory executionPlanCalculatorFactory, 
        ILogger<ExecutionPlanService> logger)
    {
        _exchangeRepository = exchangeRepository ?? throw new ArgumentNullException(nameof(exchangeRepository));
        _executionPlanCalculatorFactory = executionPlanCalculatorFactory ?? throw new ArgumentNullException(nameof(executionPlanCalculatorFactory));
        _logger = logger;
    }

    public async Task<Types.ExecutionPlan> CreateAsync(
        decimal requestedAmount,
        OrderType orderType)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(requestedAmount, nameof(requestedAmount));

        try
        {
            var exchanges = await _exchangeRepository.FetchAllExchangesAsync();

            var executionPlanCalculator = _executionPlanCalculatorFactory.Create(orderType);

            var executionPlanEntries = executionPlanCalculator.Calculate(requestedAmount, exchanges);

            return Types.ExecutionPlan.Create(executionPlanEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error occurred while creating the execution plan. RequestedAmount: {RequestedAmount}, OrderType: {OrderType}", 
                requestedAmount, 
                orderType);

            throw new ExecutionPlanCreationException(
                "An error occurred while creating the execution plan.",
                ex);
        }
    }
}