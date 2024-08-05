using System.ComponentModel.DataAnnotations;

namespace Market.APIs.Dtos
{
    public class BasketItemsDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string PictureUrl { get; set; } = null!;
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage ="Price must be grater than zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be one item at least")]
        public int Quantity { get; set; }
        [Required]
        public string Category { get; set; } = null!;
        [Required]
        public string Brand { get; set; } = null!;
    }
}