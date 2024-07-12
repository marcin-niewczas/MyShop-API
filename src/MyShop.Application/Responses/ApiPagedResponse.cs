using MyShop.Core.Abstractions;

namespace MyShop.Application.Responses;
public record ApiPagedResponse<TDto> : IApiResponse
    where TDto : class, IDto
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool IsNext { get; }
    public bool IsPrevious { get; }
    public IReadOnlyCollection<TDto> Data { get; }

    public ApiPagedResponse(IReadOnlyCollection<TDto> dtos, int totalCount, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = Convert.ToInt32(Math.Ceiling(TotalCount / (double)PageSize));
        IsNext = TotalPages > PageNumber;
        IsPrevious = TotalPages > 0 && PageNumber > 1 && PageNumber <= TotalPages + 1;
        Data = dtos;
    }
}
