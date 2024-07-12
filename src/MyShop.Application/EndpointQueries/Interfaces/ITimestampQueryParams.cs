namespace MyShop.Application.EndpointQueries.Interfaces;
public interface ITimestampQueryParams
{
    DateTimeOffset? FromDate { get; }
    DateTimeOffset? ToDate { get; }
    bool InclusiveFromDate { get; }
    bool InclusiveToDate { get; }
}
