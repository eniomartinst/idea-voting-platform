namespace CisApi.Src.Core.Domain.Exceptions;

/// <summary>
/// Base exception class for all domain-specific errors.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}