using CryptoExchangeTask.Business.Repository.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoExchangeTask.Business.ExecutionPlan.Calculators;

public interface IExecutionPlanCalculatorFactory
{
    IExecutionPlanCalculator Create(OrderType orderType);
}

internal sealed class ExecutionPlanCalculatorFactory : IExecutionPlanCalculatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ExecutionPlanCalculatorFactory(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IExecutionPlanCalculator Create(OrderType orderType) =>
        orderType switch
        {
            OrderType.Buy => _serviceProvider.GetRequiredService<BuyerExecutionPlanCalculator>(),
            OrderType.Sell => _serviceProvider.GetRequiredService<SellerExecutionPlanCalculator>(), 
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
}