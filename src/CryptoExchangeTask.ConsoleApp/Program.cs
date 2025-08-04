using CryptoExchangeTask.Business.ExecutionPlan;
using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Extensions;
using CryptoExchangeTask.Business.Repository.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(builder => builder.AddConsole());
services.AddBusinessServices();
await using var serviceProvider = services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

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

var executionPlanService = serviceProvider.GetRequiredService<IExecutionPlanService>();
var executionPlan = await executionPlanService.Create(requestedAmount, orderType);

PrintResult(requestedAmount, orderType, executionPlan);

return;

static void PrintResult(decimal requestedAmount1, OrderType orderType1, ExecutionPlan executionPlan1)
{
    Console.WriteLine();

    Console.WriteLine("Execution Plan request:");
    Console.WriteLine($" - requested amount: '{requestedAmount1}'");
    Console.WriteLine($" - requested type: '{orderType1}'");

    Console.WriteLine();

    Console.WriteLine("Created execution Plan:");
    Console.WriteLine($"Total count of orders: '{executionPlan1.TotalOrders}'");
    Console.WriteLine($"Total price in EUR: '{executionPlan1.TotalPrice}'");
    Console.WriteLine($"Total amount in crypto: '{executionPlan1.TotalAmount}'");
    Console.WriteLine("Orders:");
    foreach (var order in executionPlan1.Orders)
    {
        Console.WriteLine($" - Exchange: '{order.ExchangeId}', OrderId: '{order.OrderId}', Price: '{order.Price}', Amount: '{order.Amount}'");
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