using Market.APIs.Errors;
using Market.Core.Entities.Basket;
using Market.Core.Entities.Order_Aggregate;
using Market.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Market.APIs.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private const string endpointSecret = "whsec_9ea888bbd989a1228af14941c02b74ff866316de182ba6de35ffdf40925cec88";
        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) return BadRequest(new ApiResponse(400));

            return Ok(basket);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> Index ()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            Order order;

            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {

                order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);

                _logger.LogInformation("order is succeeded {0}", order?.PaymentIntentId);
                _logger.LogInformation("unhandled event type {0}", stripeEvent.Type);
            }
            else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);

                _logger.LogInformation("order is succeeded {0}", order?.PaymentIntentId);
                _logger.LogInformation("unhandled event type {0}", stripeEvent.Type);

            }
            return Ok();



        }

    }
}
