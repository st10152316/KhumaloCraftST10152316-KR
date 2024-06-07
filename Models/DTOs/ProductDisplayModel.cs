namespace KhumaloCraftST10152316.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductType> ProductTypes { get; set; }
        public string STerm { get; set; } = "";
        public int ProductTypeId { get; set; } = 0;
    }
}
