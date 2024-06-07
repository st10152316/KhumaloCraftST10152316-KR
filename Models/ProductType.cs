using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhumaloCraftST10152316.Models
{
    [Table("ProductType")]
    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string ProductTypeName { get; set; }
        public List<Product> Products { get; set; }
    }
}
