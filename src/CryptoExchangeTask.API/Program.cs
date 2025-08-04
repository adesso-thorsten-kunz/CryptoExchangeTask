using CryptoExchangeTask.API.Contracts;
using CryptoExchangeTask.Business.ExecutionPlan;
using CryptoExchangeTask.Business.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessServices();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/execution-plan", async ([AsParameters] ExecutionPlanRequest request, IExecutionPlanService executionPlanService) =>
    {
        if (!Enum.IsDefined(typeof(OrderType), request.OrderType))
            return Results.BadRequest("Invalid OrderType");

        var executionPlan = await executionPlanService.Create(request.Amount, MapOrderType(request.OrderType));

        return Results.Ok(executionPlan);

        CryptoExchangeTask.Business.Repository.Types.OrderType MapOrderType(OrderType orderType)
        {
            return orderType switch
            {
                OrderType.Buy => CryptoExchangeTask.Business.Repository.Types.OrderType.Buy,
                OrderType.Sell => CryptoExchangeTask.Business.Repository.Types.OrderType.Sell,
                _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
            };
        }
    })
.WithName("GetExecutionPlan")
.WithOpenApi();

app.Run();