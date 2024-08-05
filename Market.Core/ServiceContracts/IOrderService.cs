using Market.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.ServiceContracts
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail,string basketId, int deliveryMethodId, Address address);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();


    }
}
