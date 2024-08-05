using Market.Core;
using Market.Core.Entities.Basket;
using Market.Core.Entities.Order_Aggregate;
using Market.Core.RepositoriesContracts;
using Market.Core.ServiceContracts;
using Market.Core.Specification.Order_Specs;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Market.Core.Entities.Product.Product;

namespace Market.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            decimal shippingPrice = 0m;
            
            if(basket.DeliveryMethodId.HasValue)
            {
               var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

                shippingPrice = deliveryMethod.Cost;

                basket.ShippingPrice = shippingPrice;
            }

            if (basket.Items.Count > 0)
            {
                 var productRepo = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id);
                    if (product == null) return null;

                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService= new PaymentIntentService();

            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Price * 100 * i.Quantity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Price * 100 * i.Quantity) + (long)shippingPrice * 100
                };

                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateBasketAsync(basket);
            return basket;

        }

        public async Task<Order> UpdateOrderStatus(string paymnetIntent, bool isPaid)
        {
            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecificationWithPaymentIntent(paymnetIntent);

            var order = await orderRepo.GetByIdWithSpecAsync(spec);

            if (order is null) return null;

            if (isPaid)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            orderRepo.Update(order);
            await _unitOfWork.CompleteAsync();
            return order;

        }
    }
}
