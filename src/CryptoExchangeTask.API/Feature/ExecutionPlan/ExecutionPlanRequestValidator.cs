using CryptoExchangeTask.API.Contracts.ExecutionPlan;
using FluentValidation;

namespace CryptoExchangeTask.API.Feature.ExecutionPlan;

public class ExecutionPlanRequestValidator : AbstractValidator<ExecutionPlanRequest>
{
    public ExecutionPlanRequestValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.OrderType).IsInEnum();
    }
}