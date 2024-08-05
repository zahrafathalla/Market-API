namespace Market.Core.Entities.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; } = null!;
        public List<BasketItems> Items { get; set; } = null!;
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<BasketItems>();
        }
    }
}
