using Market.APIs.Errors;
using Market.APIs.Extenstions;
using Market.APIs.Helpers;
using Market.APIs.MiddleWares;
using Market.Core.Entities.Identity;
using Market.Core.RepositoriesContracts;
using Market.Repository;
using Market.Repository._Identity;
using Market.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace Market.APIs
{
    public class Program
    {
        public static async Task  Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the Dependency Injection container.

            #region Configure service
            //webApplicationBuilder : instance of WebApplication class
            //Services: property provide access to DI container
            //AddControllers : register Controllers to DI

            builder.Services.AddControllers().AddNewtonsoftJson(option=>
            {
                option.SerializerSettings.ReferenceLoopHandling= Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            //register required service of swagger to DI
            builder.Services.AddSwaggerService();

            builder.Services.AddDbContext<MarketDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<MarketIdentityDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddApplicationService();

            builder.Services.AddScoped<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policyOptions =>
                {
                    policyOptions.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });

            #endregion

            var app = builder.Build();

            #region Update Migration
            using var scope = app.Services.CreateScope(); // create scope for services that registerd in DI container (configure service)

            var services = scope.ServiceProvider; // create dependency (obj) from all services in the scope 

            var _dbContext = services.GetRequiredService<MarketDBContext>();

            var _identityDbContext = services.GetRequiredService<MarketIdentityDBContext>();

            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync();

                await _identityDbContext.Database.MigrateAsync();

                await MarketContextSeed.SeedAsync(_dbContext);
                await MarketIdentityContextSeed.SeedUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during apply the migration");

            } 
            #endregion

            // Configure the HTTP request pipeline.
             #region configure

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                //middleware to add Swagger support to your application.
                app.UseSwagger();
                //middleware to set up the Swagger UI 
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            //middleware to ensure that all incoming HTTP requests are automatically redirected to their HTTPS
            app.UseHttpsRedirection();

            // (in .net 5) use routing : match request to endpoint
            //(in .net 5) use endpoint : execute matched endpoints
            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            //When a request arrives, the routing system examines the URL path.
            //It matches the path to a registered route(defined by the controller and action method).
            //The associated controller action method processes the request and returns an appropriate response.
            app.MapControllers();

            app.UseAuthentication();

            app.UseAuthorization();
            #endregion

            app.Run();
        }
    }
}
