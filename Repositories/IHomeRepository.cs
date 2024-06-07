namespace KhumaloCraftST10152316
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int ProductTypeId = 0);
        Task<IEnumerable<ProductType>> ProductTypes();
    }
}