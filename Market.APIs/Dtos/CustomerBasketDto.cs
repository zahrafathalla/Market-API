using Market.Core.Entities.Basket;
using System.ComponentModel.DataAnnotations;

namespace Market.APIs.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } = null!;
        public string? PaymentIntentId { get; set; } 
        public string? ClientSecret { get; set; }
        public decimal ShippingPrice { get; set; }
        public int? DeliveryMethodId { get; set; }
        [Required]
        public List<BasketItemsDto> Items { get; set; } = null!;
    }
}
