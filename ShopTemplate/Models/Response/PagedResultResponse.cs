using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.Models.Response;

public class PagedResultResponse<T>
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public List<T>? Items { get; set; }

    public static PagedResultResponse<TResponse> FromDto<TDto, TResponse>(PagedResultDto<TDto> dto,
        Func<TDto, TResponse> mapFunc)
    {
        return new PagedResultResponse<TResponse>
        {
            TotalCount = dto.TotalCount,
            PageSize = dto.PageSize,
            PageNumber = dto.PageNumber,
            Items = dto.Items?.Select(mapFunc).ToList()
        };
    }
    
}