using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Data
{
    public interface IKrkaRepo
    {
        bool SaveChanges();
        IEnumerable<Skladisce> GetAllSkladisca();
        Skladisce GetSkladisceByPrefix(string prefix);
        IEnumerable<TockaSkladisca> GetAllTockeSkladisca(int skladisceId);
        IEnumerable<Dostava> GetAllDostave(DateTime start, DateTime end, int skladisceId);
        void AddDostava(Dostava dostava);
        Dostava GetDostava(string sifraDostave);
    }

    public class KrkaRepo : IKrkaRepo
    {
        private readonly KrkaDbContext _context;

        public KrkaRepo(KrkaDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Skladisce> GetAllSkladisca()
        {
            return _context.Skladisca.AsNoTracking().ToList();
        }

        public Skladisce GetSkladisceByPrefix(string prefix)
        {
            return _context.Skladisca.AsNoTracking().SingleOrDefault(t => t.Sifra.StartsWith(prefix));
        }

        public IEnumerable<TockaSkladisca> GetAllTockeSkladisca(int skladisceId)
        {
            return _context.TockeSkladisc.AsNoTracking().Where(t => t.SkladisceId == skladisceId).ToList();
        }

        public IEnumerable<Dostava> GetAllDostave(DateTime start, DateTime end, int skladisceId)
        {
            return _context.Dostave.AsNoTracking().Where(t => t.Termin >= start && t.Termin <= end && t.TockaSkladisca.SkladisceId == skladisceId).ToList();
        }

        public void AddDostava(Dostava dostava)
        {
            _context.Dostave.Add(dostava);
        }

        public Dostava GetDostava(string sifraDostave)
        {
            return _context.Dostave
                .AsNoTracking()
                .Include(t => t.Podjetje)
                .Include(t => t.TockaSkladisca)
                .Include(t => t.TockaSkladisca.Skladisce)
                .SingleOrDefault(t => t.Sifra == sifraDostave);
        }
    }
}
