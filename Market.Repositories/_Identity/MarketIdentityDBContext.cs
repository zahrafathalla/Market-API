using Market.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repository._Identity
{
    public class MarketIdentityDBContext : IdentityDbContext<AppUser>
    {

        public MarketIdentityDBContext(DbContextOptions<MarketIdentityDBContext> options)
          : base(options)
        {
            
        }
    }
}
