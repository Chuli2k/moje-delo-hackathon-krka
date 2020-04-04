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
        IEnumerable<Dostava> GetAllDostave(DateTime start, DateTime end, int? skladisceId = null);
        void AddDostava(Dostava dostava);
        Dostava GetDostava(string sifraDostave);
        void DeleteDostava(int id);
    }

    public class KrkaRepo : IKrkaRepo
    {
        private readonly KrkaDbContext _context;

        public KrkaRepo(KrkaDbContext context)
        {
            _context = context;
        }

        private IQueryable<Dostava> GetDostaveAllIncluding()
        {
            return _context.Dostave
               .Include(t => t.Uporabnik)
               .Include(t => t.Podjetje)
               .Include(t => t.TockaSkladisca)
               .Include(t => t.TockaSkladisca.Skladisce);
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

        public IEnumerable<Dostava> GetAllDostave(DateTime start, DateTime end, int? skladisceId = null)
        {
            var q = GetDostaveAllIncluding().AsNoTracking();
            
            if (skladisceId.HasValue)
                q = q.Where(t => t.TockaSkladisca.SkladisceId == skladisceId);

            return q.Where(t => t.Termin >= start && t.Termin <= end).ToList();
        }

        public void AddDostava(Dostava dostava)
        {
            _context.Dostave.Add(dostava);
        }

        public Dostava GetDostava(string sifraDostave)
        {
            return GetDostaveAllIncluding()
                .AsNoTracking()
                .SingleOrDefault(t => t.Sifra == sifraDostave);
        }

        public void DeleteDostava(int id)
        {
            var d = _context.Dostave.SingleOrDefault(t => t.Id == id);
            if (d == null) return;

            _context.Dostave.Remove(d);
        }
    }
}
