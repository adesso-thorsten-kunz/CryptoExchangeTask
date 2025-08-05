namespace CryptoExchangeTask.Business.ExecutionPlan.Exceptions;

internal sealed class ExecutionPlanCreationException : Exception
{
    public ExecutionPlanCreationException()
        : base("An error occurred while creating the execution plan.")
    {
    }

    public ExecutionPlanCreationException(string message)
        : base(message)
    {
    }

    public ExecutionPlanCreationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}