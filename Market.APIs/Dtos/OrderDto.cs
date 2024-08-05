using Market.Core.Entities.Order_Aggregate;
using System.ComponentModel.DataAnnotations;

namespace Market.APIs.Dtos
{
    public class OrderDto
    {
       // public string BuyerEmail { get; set; } = null!;
        [Required]
        public string BasketId { get; set; } = null!;
        [Required]
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; } = null!;
       
    }
}
