namespace MyShop.Application.EndpointQueries.Interfaces;
public interface ISortQueryParams
{
    string? SortBy { get; }
    string? SortDirection { get; }
}
