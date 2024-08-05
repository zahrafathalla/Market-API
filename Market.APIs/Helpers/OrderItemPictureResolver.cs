using AutoMapper;
using Market.APIs.Dtos;
using Market.Core.Entities.Order_Aggregate;

namespace Market.APIs.Helpers
{
    public class OrderItemPictureResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureURL))
                return $"{_configuration["BaseURL"]}/{source.Product.PictureURL}";

            return string.Empty;
        }
    }
}
