using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Market.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        private Order() { }

        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod? deliveryMethod, ICollection<OrderItem> items, decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; } = null!;
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set;} = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }
        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; } 

    }
}
