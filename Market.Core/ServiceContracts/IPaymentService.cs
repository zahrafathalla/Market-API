using Market.Core.Entities.Basket;
using Market.Core.Entities.Order_Aggregate;

namespace Market.Core.ServiceContracts
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdateOrderStatus(string paymnetIntent, bool isPaid);
    }
}
