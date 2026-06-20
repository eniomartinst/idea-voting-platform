namespace CisApi.Src.Core.Domain.Exceptions;

/// <summary>
/// Exception thrown when a business rule prevents an operation.
/// </summary>
public class OperationNotAllowedException : DomainException
{
    public OperationNotAllowedException(string message) : base(message)
    {
    }
}