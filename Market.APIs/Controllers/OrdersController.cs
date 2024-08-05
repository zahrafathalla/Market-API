using AutoMapper;
using Market.APIs.Dtos;
using Market.APIs.Errors;
using Market.Core.Entities.Order_Aggregate;
using Market.Core.Entities.Product;
using Market.Core.RepositoriesContracts;
using Market.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;

namespace Market.APIs.Controllers
{
    [Authorize]

    public class OrdersController : BaseController
    {

        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(
            IOrderService orderService,
            IMapper mapper)
        {

            _orderService = orderService;
            _mapper = mapper;
        }
        //[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var address = _mapper.Map<Address>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

            if (order == null) return BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet] // /api/orders?email=          as query parameter
        public async Task<ActionResult> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetOrderForUser(int Id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(email, Id);

            if (order == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<OrderToReturnDto>(order));

        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();

            return Ok(deliveryMethods);
        }
            
    }
}
