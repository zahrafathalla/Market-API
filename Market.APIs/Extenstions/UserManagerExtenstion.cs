using Market.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Market.APIs.Extenstions
{
    public static class UserManagerExtenstion
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(u=> u.Address).FirstOrDefaultAsync(u=> u.NormalizedEmail== email.ToUpper());

            return user;
        }
    }
}
