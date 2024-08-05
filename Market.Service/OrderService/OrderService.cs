using Market.Core;
using Market.Core.Entities.Order_Aggregate;
using Market.Core.Entities.Product;
using Market.Core.RepositoriesContracts;
using Market.Core.ServiceContracts;
using Market.Core.Specification.Order_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail,string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //Get basket from basket repository
            var basket = await _basketRepository.GetBasketAsync(basketId);

            //Get selected items at basket from product repo

            var orderItems = new List<OrderItem>();
            var _productRepo = _unitOfWork.Repository<Product>();

            if (basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await _productRepo.GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // calculate subTotal
            var subtotal = orderItems.Sum(orderItem => orderItem.Price*orderItem.Quantity);

            //Get Delivery Method
            
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //create order
            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecificationWithPaymentIntent(basket.PaymentIntentId);

            var exitingOrder = await orderRepo.GetByIdWithSpecAsync(spec);

            if(exitingOrder is not null)
            {
                orderRepo.Delete(exitingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal,basket?.PaymentIntentId ??"");

             await orderRepo.AddAsync(order);

            //save changes
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return null;

            return order;
            

        }
        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecification(buyerEmail);

            var orders = orderRepo.GetAllWithSpecAsync(spec);

            return orders;

        }

        public Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecification(buyerEmail, orderId);

            var order = orderRepo.GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }
}
