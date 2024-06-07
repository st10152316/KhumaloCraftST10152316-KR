

using Microsoft.EntityFrameworkCore;

namespace KhumaloCraftST10152316.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProductType>> ProductTypes()
        {
            return await _db.ProductTypes.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int ProductTypeId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> Products = await (from Product in _db.Products
                         join ProductType in _db.ProductTypes
                         on Product.ProductTypeId equals ProductType.Id
                         join stock in _db.Stocks
                         on Product.Id equals stock.ProductId
                         into Product_stocks
                         from ProductWithStock in Product_stocks.DefaultIfEmpty()
                         where string.IsNullOrWhiteSpace(sTerm) || (Product != null && Product.ProductName.ToLower().StartsWith(sTerm))
                         select new Product
                         {
                             Id = Product.Id,
                             Image = Product.Image,
                             ProductDescription = Product.ProductDescription,
                             ProductName = Product.ProductName,
                             ProductTypeId = Product.ProductTypeId,
                             Price = Product.Price,
                             ProductTypeName = ProductType.ProductTypeName,
                             Quantity=ProductWithStock==null? 0:ProductWithStock.Quantity
                         }
                         ).ToListAsync();
            if (ProductTypeId > 0)
            {

                Products = Products.Where(a => a.ProductTypeId == ProductTypeId).ToList();
            }
            return Products;

        }
    }
}
