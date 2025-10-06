using Microsoft.EntityFrameworkCore;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;

namespace ShopTemplate.DB.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ShopContext _dbContext;

    public ProductRepository(ShopContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProductDto>> GetAll()
    {
        var products = await _dbContext.Products.ToListAsync();
        return products.Select(x => x.ToDto());
    }

    public async Task<ProductDto?> GetById(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        return product?.ToDto();
    }

    public async Task<ProductDto?> GetByName(string name)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        return product?.ToDto();
    }

    public async Task Add(ProductDto dto)
    {
        var product = new Product().FromDto(dto);
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
    }

    public Task Update(ProductDto product)
    {
        throw new NotImplementedException();
    }

    public Task Delete(ProductDto product)
    {
        throw new NotImplementedException();
    }

    public IQueryable<ProductDto> GetAllQueryable()
    {
        var quereble =  _dbContext.Products.AsQueryable();
        return quereble.Select(x => x.ToDto());
    }
    
    public async Task<PagedResultDto<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize)
    {
        const int maxItems = 500;

        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _dbContext.Products.AsQueryable();

        var totalCount = await query.CountAsync();
        
        var maxPageSize = Math.Min(pageSize, maxItems);
        var skip = (pageNumber - 1) * maxPageSize;
        
        if (skip >= maxItems)
        {
            skip = maxItems - maxPageSize;
            if (skip < 0) skip = 0;
        }

        var items = await query
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(maxPageSize)
            .Select(p => p.ToDto())
            .ToListAsync();

        return new PagedResultDto<ProductDto>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = maxPageSize,
            Items = items
        };
    }
}