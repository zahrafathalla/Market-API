using AutoMapper;
using AutoMapper.Execution;
using Market.APIs.Dtos;
using Market.Core.Entities.Product;

namespace Market.APIs.Helpers
{
    public class PictureResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public PictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["BaseURL"]}/{source.PictureUrl}";

            return string.Empty;
        }
    }
}
