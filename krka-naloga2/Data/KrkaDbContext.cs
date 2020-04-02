using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Data
{
    public class KrkaDbContext : IdentityDbContext<IdentityUser>
    {
        public KrkaDbContext(DbContextOptions<KrkaDbContext> options)
            : base(options)
        {
        }
    }
}
