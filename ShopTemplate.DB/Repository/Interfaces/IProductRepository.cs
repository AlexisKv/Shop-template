using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.DB.Repository.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetAll();
    Task<ProductDto?> GetById(int id);
    Task<ProductDto?> GetByName(string name);
    Task Add(ProductDto product);
    Task Update(ProductDto product);
    Task Delete(ProductDto product);
    IQueryable<ProductDto> GetAllQueryable();
    Task<PagedResultDto<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize);
}