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

        public DbSet<Uporabnik> Uporabniki { get; set; }
        public DbSet<Podjetje> Podjetja { get; set; }
        public DbSet<Skladisce> Skladisca { get; set; }
        public DbSet<TockaSkladisca> TockeSkladisc { get; set; }
        public DbSet<Dostava> Dostave { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
