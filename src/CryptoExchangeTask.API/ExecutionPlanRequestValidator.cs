using CryptoExchangeTask.API.Contracts;
using FluentValidation;

namespace CryptoExchangeTask.API;

public class ExecutionPlanRequestValidator : AbstractValidator<ExecutionPlanRequest>
{
    public ExecutionPlanRequestValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.OrderType)
            .IsInEnum()
            .WithMessage("Invalid OrderType");
    }
}