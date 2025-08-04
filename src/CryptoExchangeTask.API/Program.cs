using CryptoExchangeTask.API.Contracts;
using CryptoExchangeTask.API.ExceptionHandler;
using CryptoExchangeTask.API.Validator;
using CryptoExchangeTask.Business.ExecutionPlan;
using CryptoExchangeTask.Business.ExecutionPlan.Types;
using CryptoExchangeTask.Business.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessServices();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
builder.Services.AddValidatorsFromAssemblyContaining<ExecutionPlanRequestValidator>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>(); 
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.MapGet("/execution-plan", async (
        [AsParameters] ExecutionPlanRequest request, 
        IExecutionPlanService executionPlanService,
        IValidator<ExecutionPlanRequest> validator) =>
    {
        // ToDo: Add Swagger documentation
        await validator.ValidateAndThrowAsync(request);

        var createdExecutionPlan = await executionPlanService.Create(request.Amount, MapOrderType(request.OrderType));

        return Results.Ok(MapToResponse(createdExecutionPlan));

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