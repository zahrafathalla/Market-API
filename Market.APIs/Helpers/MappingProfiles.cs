using AutoMapper;
using Market.APIs.Dtos;
using Market.Core.Entities.Basket;
using Market.Core.Entities.Identity;
using Market.Core.Entities.Order_Aggregate;
using Market.Core.Entities.Product;
using static System.Net.WebRequestMethods;

namespace Market.APIs.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d=>d.PictureUrl, o => o.MapFrom<PictureResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemsDto, BasketItems>();


            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap(); //address for identity address
            CreateMap<AddressDto, Core.Entities.Order_Aggregate.Address>(); // address for order_aggregate

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d=> d.ProductId , o=>o.MapFrom(s=>s.Product.ProductId))
                .ForMember(d=> d.ProductName, o=>o.MapFrom(s=>s.Product.ProductName))
                .ForMember(d=> d.PictureURL, o=>o.MapFrom(s=>s.Product.PictureURL))
                .ForMember(d => d.PictureURL, o => o.MapFrom<OrderItemPictureResolver>());



        }
    }
}
