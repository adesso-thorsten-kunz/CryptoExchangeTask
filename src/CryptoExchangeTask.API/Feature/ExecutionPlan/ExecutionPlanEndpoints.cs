using CryptoExchangeTask.API.Contracts.ExecutionPlan;
using CryptoExchangeTask.Business.ExecutionPlan;
using FluentValidation;

namespace CryptoExchangeTask.API.Feature.ExecutionPlan;

public static class ExecutionPlanEndpoints
{
    public static void MapExecutionPlanEndpoints(this WebApplication app) =>
        app.MapGet("/execution-plan", async (
                [AsParameters] ExecutionPlanRequest request,
                IExecutionPlanService executionPlanService,
                IValidator<ExecutionPlanRequest> validator) =>
            {
                await validator.ValidateAndThrowAsync(request);

                var createdExecutionPlan = await executionPlanService.Create(
                    request.Amount,
                    MapOrderType(request.OrderType));

                return Results.Ok(MapToResponse(createdExecutionPlan));
            })
            .WithName("GetExecutionPlan")
            .WithOpenApi();

    private static CryptoExchangeTask.Business.Repository.Types.OrderType MapOrderType(OrderType orderType)
    {
        return orderType switch
        {
            OrderType.Buy => Business.Repository.Types.OrderType.Buy,
            OrderType.Sell => Business.Repository.Types.OrderType.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
    }

    private static ExecutionPlanResponse MapToResponse(Business.ExecutionPlan.Types.ExecutionPlan executionPlan)
    {
        return ExecutionPlanResponse.Create(
            executionPlan.Orders.Select(order => new ExecutionPlanEntry
            {
                Amount = order.Amount,
                Price = order.Price,
                ExchangeId = order.ExchangeId,
                OrderId = order.OrderId
            }).ToList().AsReadOnly());
    }
}