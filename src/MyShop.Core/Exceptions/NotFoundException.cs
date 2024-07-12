using System.Net;

namespace MyShop.Core.Exceptions;
public sealed class NotFoundException : CustomException
{
    public override int StatusCode { get; } = (int)HttpStatusCode.NotFound;

    public NotFoundException() : base(null) { }

    public NotFoundException(string? message) : base(message) { }

    public NotFoundException(string entityName, Guid id) : base($"{entityName} with {id} not found.") { }

    public NotFoundException(string entityName, IEnumerable<Guid> ids) : base($"{entityName} with [ {string.Join(", ", ids)} ] not found.") { }

    public NotFoundException(string entityName, string value) : base($"{entityName} with {value} not found.") { }
}
