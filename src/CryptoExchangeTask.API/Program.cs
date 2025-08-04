using CryptoExchangeTask.API;
using CryptoExchangeTask.API.Contracts;
using CryptoExchangeTask.Business.ExecutionPlan;
using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessServices();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
builder.Services.AddValidatorsFromAssemblyContaining<ExecutionPlanRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/execution-plan", async (
        [AsParameters] ExecutionPlanRequest request, 
        IExecutionPlanService executionPlanService,
        IValidator<ExecutionPlanRequest> validator) =>
    {
        // ToDo: Add Swagger documentation

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.Problem(
                title: "Validation failed",
                detail: "One or more validation errors occurred.",
                statusCode: 400,
                extensions: new Dictionary<string, object?>
                {
                    ["errors"] = validationResult.Errors.Select(e => new
                    {
                        e.PropertyName,
                        e.ErrorMessage
                    })
                });
        }

        var executionPlan = await executionPlanService.Create(request.Amount, MapOrderType(request.OrderType));

        return Results.Ok(MapToResponse(executionPlan));

        CryptoExchangeTask.Business.Repository.Types.OrderType MapOrderType(OrderType orderType)
        {
            return orderType switch
            {
                OrderType.Buy => CryptoExchangeTask.Business.Repository.Types.OrderType.Buy,
                OrderType.Sell => CryptoExchangeTask.Business.Repository.Types.OrderType.Sell,
                _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
            };
        }

        ExecutionPlanResponse MapToResponse(ExecutionPlan executionPlan)
        {
            return ExecutionPlanResponse.Create(
                executionPlan.Orders.Select(o => new CryptoExchangeTask.API.Contracts.ExecutionPlanEntry
                {
                    Amount = o.Amount,
                    Price = o.Price,
                    ExchangeId = o.ExchangeId,
                    OrderId = o.OrderId
                }).ToList().AsReadOnly());
        }
    })
.WithName("GetExecutionPlan")
.WithOpenApi();

app.Run();

