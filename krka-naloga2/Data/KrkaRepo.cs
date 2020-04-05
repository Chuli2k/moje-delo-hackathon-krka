using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        IEnumerable<Dostava> GetAllDostave(DateTime start, DateTime end, Expression<Func<Dostava, bool>> filter);
        IEnumerable<Dostava> GetAllDostavePaged(DateTime start, DateTime end, Expression<Func<Dostava, bool>> filter, int pageSize, int pageNum);
        void AddDostava(Dostava dostava);
        Dostava GetDostava(string sifraDostave);
        void DeleteDostava(int id);
        void SetStatusDostave(string sifraDostave, StatusDostave status);
        void UpdateDostava(Dostava dostava);
        IEnumerable<Podjetje> GetAllPodjetja();
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
            return _context.Skladisca
                .Include(t => t.TockeSkladisca)
                .AsNoTracking()
                .SingleOrDefault(t => t.Sifra.StartsWith(prefix));
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

        public IEnumerable<Dostava> GetAllDostave(DateTime start, DateTime end, Expression<Func<Dostava, bool>> filter)
        {
            var q = GetDostaveAllIncluding().AsNoTracking();

            if(filter != null) q = q.Where(filter);

            return q.OrderBy(t => t.Termin).Where(t => t.Termin >= start && t.Termin <= end).ToList();
        }

        public IEnumerable<Dostava> GetAllDostavePaged(DateTime start, DateTime end, Expression<Func<Dostava, bool>> filter, int pageSize, int pageNum)
        {
            var q = GetDostaveAllIncluding().AsNoTracking();

            if (filter != null) q = q.Where(filter);
            q = q.Where(t => t.Termin >= start && t.Termin <= end);

            return q.OrderBy(t => t.Termin).Skip(pageSize * pageNum).Take(pageSize).ToList();
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

        public void SetStatusDostave(string sifraDostave, StatusDostave status)
        {
            var d = _context.Dostave.SingleOrDefault(t => t.Sifra == sifraDostave);
            if (d == null) return;

            d.Status = status;
        }

        public void UpdateDostava(Dostava dostava)
        {
            var dostavaDb = _context.Dostave.SingleOrDefault(t => t.Id == dostava.Id);
            if (dostavaDb == null) return;

            if (dostavaDb.Sifra != dostava.Sifra)
                throw new Exception("Šifre dostave ni dovoljeno spreminjat.");

            dostavaDb.PodjetjeId = dostava.PodjetjeId;
            dostavaDb.Status = dostava.Status;
            dostavaDb.Termin = dostava.Termin;
            dostavaDb.TockaSkladiscaId = dostava.TockaSkladiscaId;
            dostavaDb.UporabnikId = dostava.UporabnikId;
        }

        public IEnumerable<Podjetje> GetAllPodjetja()
        {
            return _context.Podjetja.AsNoTracking().ToList();
        }

        public Podjetje GetPodjetje(int id)
        {
            return _context.Podjetja.AsNoTracking().SingleOrDefault(t => t.Id == id);
        }
    }
}
