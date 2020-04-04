using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Data
{
    public class KrkaDbContext : IdentityDbContext<Uporabnik>
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

            builder.Entity<Skladisce>()
                .HasIndex(t => t.Sifra)
                .IsUnique();

            builder.Entity<TockaSkladisca>()
                .HasIndex(t => new { t.SkladisceId, t.Sifra })
                .IsUnique();

            builder.Entity<Podjetje>()
                .HasIndex(t => t.Sifra)
                .IsUnique();

            builder.Entity<Dostava>()
                .HasIndex(t => t.Sifra)
                .IsUnique();

            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<Podjetje>()
                .HasData(
                    new Podjetje() { Id = 1, Naziv = "Krka", Sifra = "10001" },
                    new Podjetje() { Id = 2, Naziv = "Bayer", Sifra = "10002" }
                );

            var skl = new[]
            {
                new Skladisce() { Id = 1, Sifra = "1" },
                new Skladisce() { Id = 2, Sifra = "2" },
                new Skladisce() { Id = 3, Sifra = "3" },
            };
            var tck = new List<TockaSkladisca>();
            int i = 1;
            foreach (var s in skl)
            {
                tck.AddRange(new[] {
                    new TockaSkladisca(){ Id = i, Sifra = "1", SkladisceId = s.Id },
                    new TockaSkladisca(){ Id = i + 1, Sifra = "2", SkladisceId = s.Id },
                    new TockaSkladisca(){ Id = i + 2, Sifra = "3", SkladisceId = s.Id },
                    new TockaSkladisca(){ Id = i + 3, Sifra = "4", SkladisceId = s.Id },
                    new TockaSkladisca(){ Id = i + 4, Sifra = "5", SkladisceId = s.Id },
                });
                i += 5;
            }
            builder.Entity<Skladisce>().HasData(skl);
            builder.Entity<TockaSkladisca>().HasData(tck);
        }
    }
}
