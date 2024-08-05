namespace Market.APIs.Extenstions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            //register required service of swagger to DI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
