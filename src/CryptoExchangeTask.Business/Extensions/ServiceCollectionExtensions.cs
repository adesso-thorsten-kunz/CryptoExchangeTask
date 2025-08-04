using CryptoExchangeTask.Business.ExecutionPlan;
using CryptoExchangeTask.Business.ExecutionPlan.Calculators;
using CryptoExchangeTask.Business.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoExchangeTask.Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IExecutionPlanService, ExecutionPlanService>();
        serviceCollection.AddScoped<IExchangeRepository, JsonExchangeRepository>();
        serviceCollection.AddScoped<IExchangeJsonSerializer, ExchangeJsonSerializer>();

        serviceCollection.AddScoped<IExecutionPlanCalculatorFactory, ExecutionPlanCalculatorFactory>();
        serviceCollection.AddScoped<BuyerExecutionPlanCalculator>();
        serviceCollection.AddScoped<SellerExecutionPlanCalculator>();

        return serviceCollection;
    }
}