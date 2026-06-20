namespace CisApi.Src.Core.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : DomainException
{
    public string Resource { get; }
    public string ResourceId { get; }

    public NotFoundException(string resource, string resourceId)
        : base($"{resource} with id {resourceId} was not found.")
    {
        Resource = resource;
        ResourceId = resourceId;
    }
}