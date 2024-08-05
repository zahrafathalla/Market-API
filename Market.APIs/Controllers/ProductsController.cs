using AutoMapper;
using Market.APIs.Dtos;
using Market.APIs.Errors;
using Market.APIs.Helpers;
using Market.Core.Entities.Product;
using Market.Core.RepositoriesContracts;
using Market.Core.ServiceContracts;
using Market.Core.Specification;
using Market.Core.Specification.Product_Specs;
using Market.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.APIs.Controllers
{

    public class ProductsController : BaseController
    {

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProducts([FromQuery]ProductSpecParameters specParams)
        {          
            var products = await _productService.GetAllProductsAsync(specParams);

            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            var count = await _productService.GetProductCountAsync(specParams);

            return Ok(new Pagination<ProductToReturnDto> (specParams.PageSize, specParams.pageIndex, count, data ));
        }

        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {          
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
            =>Ok( await _productService.GetAllBrandsAsync());

        [HttpGet("categories")]
        public async Task <ActionResult<IReadOnlyList<ProductCategory>>> GetAllCategories()
            =>Ok(await _productService.GetAllCategoriesAsync());
    }
}
