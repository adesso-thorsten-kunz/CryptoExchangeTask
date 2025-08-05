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

                var createdExecutionPlan = await executionPlanService.CreateAsync(
                    request.Amount,
                    MapOrderType(request.OrderType));

                return Results.Ok(MapToResponse(createdExecutionPlan));
            })
            .WithName("GetExecutionPlan")
            .WithOpenApi();

    private static CryptoExchangeTask.Business.Repository.Types.OrderType MapOrderType(OrderType orderType) =>
        orderType switch
        {
            OrderType.Buy => Business.Repository.Types.OrderType.Buy,
            OrderType.Sell => Business.Repository.Types.OrderType.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };

    private static ExecutionPlanResponse MapToResponse(Business.ExecutionPlan.Types.ExecutionPlan executionPlan) =>
        ExecutionPlanResponse.Create(
            executionPlan.Orders.Select(order => 
                    new OrderBookEntry
                    {
                        ExchangeId = order.ExchangeId,
                        OrderId = order.OrderId,
                        Time = order.Time,
                        Type = MapOrderType(order.Type),
                        Kind = MapOrderKind(order.Kind),
                        Amount = order.Amount,
                        Price = order.Price
                    })
                .ToList()
                .AsReadOnly());

    private static OrderType MapOrderType(CryptoExchangeTask.Business.Repository.Types.OrderType orderType) =>
        orderType switch
        {
            Business.Repository.Types.OrderType.Buy => OrderType.Buy,
            Business.Repository.Types.OrderType.Sell => OrderType.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };

    private static OrderKind MapOrderKind(CryptoExchangeTask.Business.Repository.Types.OrderKind orderKind) =>
        orderKind switch
        {
            Business.Repository.Types.OrderKind.Limit => OrderKind.Limit,
            _ => throw new ArgumentOutOfRangeException(nameof(orderKind), orderKind, null)
        };
}