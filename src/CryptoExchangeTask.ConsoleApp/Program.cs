using CryptoExchangeTask.Business.ExecutionPlan;
using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Extensions;
using CryptoExchangeTask.Business.Repository.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(builder => builder.AddConsole());
services.AddBusinessServices();

OrderType orderType;
orderType = GetUserInput(
    "Please enter the order type (buy/sell): ",
    value => 
        Enum.TryParse(value, true, out orderType) ? 
            (true, orderType) : 
            (false, default));

decimal requestedAmount;
requestedAmount = GetUserInput(
    "Please enter the amount of crypto: ",
    value =>
        decimal.TryParse(value, out requestedAmount) ?
            (true, requestedAmount) :
            (false, 0));

try
{
    var executionPlan = await CreateExecutionPlan(requestedAmount, orderType, services);
    PrintResult(requestedAmount, orderType, executionPlan);
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while processing your request. {ex.Message}");
}

return;

static async Task<ExecutionPlan> CreateExecutionPlan(decimal requestedAmount, OrderType orderType, ServiceCollection services)
{
    await using var serviceProvider = services.BuildServiceProvider();
    var executionPlanService = serviceProvider.GetRequiredService<IExecutionPlanService>();
    var executionPlan = await executionPlanService.CreateAsync(requestedAmount, orderType);
    return executionPlan;
}

static void PrintResult(decimal requestedAmount, OrderType orderType, ExecutionPlan executionPlan)
{
    Console.WriteLine();

    Console.WriteLine("Execution Plan request:");
    Console.WriteLine($"Requested amount: '{requestedAmount}'");
    Console.WriteLine($"Requested type: '{orderType}'");

    Console.WriteLine();

    Console.WriteLine("Created execution Plan:");
    Console.WriteLine($"Total count of orders: '{executionPlan.TotalOrders}'");
    Console.WriteLine($"Total price in EUR: '{executionPlan.TotalPrice}'");
    Console.WriteLine($"Total amount in crypto: '{executionPlan.TotalAmount}'");
    Console.WriteLine("Orders:");
    
    foreach (var order in executionPlan.Orders)
    {
        Console.WriteLine($"Exchange: '{order.ExchangeId}'");
        Console.WriteLine($"OrderId: '{order.OrderId}'");
        Console.WriteLine($"Time: '{order.Time}'");
        Console.WriteLine($"Type: '{order.Type}'");
        Console.WriteLine($"Kind: '{order.Kind}'");
        Console.WriteLine($"Price: '{order.Price}'");
        Console.WriteLine($"Amount: '{order.Amount}'");
        Console.WriteLine();
    }
}

static T GetUserInput<T>(
    string prompt,
    Func<string, (bool, T)> tryParseUserInputFunc)
{
    while (true)
    {
        Console.Write(prompt);
        var userInput = Console.ReadLine()?.Trim();

        if (!string.IsNullOrEmpty(userInput))
        {
            var (tryParseResult, value) = tryParseUserInputFunc(userInput);

            if (tryParseResult)
            {
                return value;
            }
        }

        Console.WriteLine("Invalid input. Please try again.");
    }
}