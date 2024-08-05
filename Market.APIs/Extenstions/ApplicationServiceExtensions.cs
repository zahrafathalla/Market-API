using Market.APIs.Errors;
using Market.APIs.Helpers;
using Market.Core;
using Market.Core.Entities.Identity;
using Market.Core.RepositoriesContracts;
using Market.Core.ServiceContracts;
using Market.Repository;
using Market.Repository._Identity;
using Market.Repository.BasketRepository;
using Market.Repository.GenericRepository;
using Market.Service.AuthService;
using Market.Service.OrderService;
using Market.Service.PaymentService;
using Market.Service.ProductService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Market.APIs.Extenstions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IProductService),typeof(ProductService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
           
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddIdentity<AppUser, IdentityRole>()
                     .AddEntityFrameworkStores<MarketIdentityDBContext>();
    
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                         .SelectMany(E => E.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });


            return services;
        }

    }
}
