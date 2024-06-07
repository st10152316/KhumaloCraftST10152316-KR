using Microsoft.EntityFrameworkCore;

namespace KhumaloCraftST10152316.Repositories;

public interface IProductTypeRepository
{
    Task AddProductType(ProductType ProductType);
    Task UpdateProductType(ProductType ProductType);
    Task<ProductType?> GetProductTypeById(int id);
    Task DeleteProductType(ProductType ProductType);
    Task<IEnumerable<ProductType>> GetProductTypes();
}
public class ProductTypeRepository : IProductTypeRepository
{
    private readonly ApplicationDbContext _context;
    public ProductTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddProductType(ProductType ProductType)
    {
        _context.ProductTypes.Add(ProductType);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateProductType(ProductType ProductType)
    {
        _context.ProductTypes.Update(ProductType);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductType(ProductType ProductType)
    {
        _context.ProductTypes.Remove(ProductType);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductType?> GetProductTypeById(int id)
    {
        return await _context.ProductTypes.FindAsync(id);
    }

    public async Task<IEnumerable<ProductType>> GetProductTypes()
    {
        return await _context.ProductTypes.ToListAsync();
    }

    
}
