using Market.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repository._Identity
{
    public static class MarketIdentityContextSeed
    {

        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "zahra gamal",
                    Email = "zahra@gmail.com",
                    UserName = "zahra.gamal",
                    PhoneNumber = "1234"
                }; 

                await userManager.CreateAsync(user,"P@ssw0rd");
            }


        }
    }
}
