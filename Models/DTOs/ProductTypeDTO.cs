using System.ComponentModel.DataAnnotations;

namespace KhumaloCraftST10152316.Models.DTOs
{
    public class ProductTypeDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string ProductTypeName { get; set; }
    }
}
