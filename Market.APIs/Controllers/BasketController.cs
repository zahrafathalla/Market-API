using AutoMapper;
using Market.APIs.Dtos;
using Market.APIs.Errors;
using Market.Core.Entities.Basket;
using Market.Core.RepositoriesContracts;
using Market.Repository.BasketRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.APIs.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketRepository _basketRepos;
        private readonly IMapper _mapper;

        public BasketController(
            IBasketRepository basketRepos ,
            IMapper mapper 
            )
        {
            _basketRepos = basketRepos;
            _mapper = mapper;
        }

        [HttpGet] 
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {

            var basket = await _basketRepos.GetBasketAsync(id);

            return Ok(basket is null ? NotFound(new ApiResponse(404)) : basket);

        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasket>(basket);
            var createdOrUpdated = await _basketRepos.UpdateBasketAsync(mappedBasket);

            if (createdOrUpdated is null)
                return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpdated);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var basket = await _basketRepos.DeleteBasketAsync(id);

            return Ok(basket);
        }
    }
}
