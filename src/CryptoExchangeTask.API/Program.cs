using CryptoExchangeTask.API;
using CryptoExchangeTask.API.ExceptionHandler;
using CryptoExchangeTask.API.Feature.ExecutionPlan;
using CryptoExchangeTask.Business.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessServices();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>(); 
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.MapExecutionPlanEndpoints();

app.Run();