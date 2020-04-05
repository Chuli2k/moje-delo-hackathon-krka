using krka_naloga2.Data;
using krka_naloga2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace krka_naloga2.Shared
{
    public class DostavaDataManager : IDostavaDataManager
    {
        private readonly IKrkaRepo _krkaRepo;
        private readonly UserManager<Uporabnik> _userManager;

        public DostavaDataManager(
            IKrkaRepo krkaRepo,
            UserManager<Uporabnik> userManager)
        {
            _krkaRepo = krkaRepo;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Dostava>> GetAllSeznamDostavAsync(DateTime from, DateTime to, Uporabnik uporabnik)
        {
            if (uporabnik == null)
                throw new ArgumentNullException(nameof(uporabnik));
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            var roles = await _userManager.GetRolesAsync(uporabnik);

            Expression<Func<Dostava, bool>> filter = d => false; //Ne vrni nič

            if (roles.Contains("Admin"))
            {
                filter = null; //Vrni vse
            } 
            else if(roles.Contains("Uporabnik"))
            {
                filter = d => d.UporabnikId == uporabnik.Id; //Vrni samo dostave za uporabnik
            } 
            else if (roles.Contains("Skladiscnik"))
            {
                //Vse planirane dostave za skladišče
                filter = d => d.TockaSkladisca.SkladisceId == uporabnik.SkladisceId && d.Status == StatusDostave.Planiran;
            }

            var dostave = _krkaRepo.GetAllDostave(from, to, filter);

            return dostave;
        }

        public Skladisce GetSkladisceFromSifraDostave(string sifraDostave)
        {
            var skl = _krkaRepo.GetSkladisceByPrefix(sifraDostave[0].ToString());
            return skl;
        }

        public Dostava GetDostava(string sifraDostave)
        {
            return _krkaRepo.GetDostava(sifraDostave);
        }

        public async Task<string> CheckEditDostavaAsync(Dostava dostava, Uporabnik uporabnik)
        {
            if (dostava == null) throw new ArgumentNullException(nameof(dostava));
            if (uporabnik == null) throw new ArgumentNullException(nameof(uporabnik));

            if (dostava.Status == StatusDostave.Potrjen)
                return "Dostava je že potrjena.";

            var roles = await _userManager.GetRolesAsync(uporabnik);

            //Urejaš lahko samo svoje dostave, razen, če si Admin
            if (dostava.UporabnikId != uporabnik.Id && !roles.Contains("Admin"))
                return "Dostave ni mogoče urejati.";

            return null;
        }

        public IEnumerable<TerminTockeSkladisca> GetAllZasedeniTermini(DateTime start, DateTime end, int skladisceId)
        {
            var termini = new List<TerminTockeSkladisca>();
            var terminiDb = _krkaRepo.GetAllDostave(start, end, skladisceId);
            if (terminiDb.Count() != 0)
                termini.AddRange(terminiDb.Select(t => new TerminTockeSkladisca() { Termin = t.Termin, TockaSkladiscaId = t.TockaSkladiscaId }));

            return termini;
        }

        public async Task<IEnumerable<Uporabnik>> GetAllUporabnikiForVnosDostaveAsync()
        {
            var podjetja = _krkaRepo.GetAllPodjetja();
            var users = new List<Uporabnik>();
            users.AddRange(await _userManager.GetUsersInRoleAsync("Uporabnik"));
            users.AddRange(await _userManager.GetUsersInRoleAsync("Admin"));

            //User manager ne napolni relacije, zaradi tega tu ročno polnim
            foreach (var uporabnik in users)
            {
                uporabnik.Podjetje = podjetja.SingleOrDefault(p => p.Id == uporabnik.PodjetjeId);
            }

            var ret = users.Select(t => new SelectListItem()
            {
                Value = t.Id.ToString(),
                Text = $"{t.UserName} ({podjetja.SingleOrDefault(p => p.Id == t.PodjetjeId)?.Naziv})"
            });

            return users;
        }

        public async Task UrediDostavoAsync(IzberiterminDostaveModel podatki, Uporabnik uporabnikPrijave)
        {
            var dostavaDb = _krkaRepo.GetDostava(podatki.SifraDostave);
            var uporabnikIzbire = await _userManager.FindByIdAsync(podatki.IzbranUporabnikId);
            var roles = await _userManager.GetRolesAsync(uporabnikPrijave);

            if (roles.Contains("Admin"))
            {
                //Lahko ureja uporabnika
                dostavaDb.UporabnikId = uporabnikIzbire.Id;
                dostavaDb.PodjetjeId = uporabnikIzbire.PodjetjeId;
            }

            dostavaDb.TockaSkladiscaId = podatki.IzbranaTockaId.Value;
            dostavaDb.Termin = podatki.IzbranDatum.Value.AddHours(podatki.IzbranaUra.Value);

            _krkaRepo.UpdateDostava(dostavaDb);
            _krkaRepo.SaveChanges();
        }

        public async Task DodajDostavoAsync(IzberiterminDostaveModel podatki, Uporabnik uporabnikPrijave)
        {
            var uporabnikIzbire = await _userManager.FindByIdAsync(podatki.IzbranUporabnikId);
            var roles = await _userManager.GetRolesAsync(uporabnikPrijave);

            //Dodajanje dostave
            var dostava = new Dostava()
            {
                PodjetjeId = uporabnikPrijave.PodjetjeId,
                UporabnikId = uporabnikPrijave.Id,
                Sifra = podatki.SifraDostave,
                TockaSkladiscaId = podatki.IzbranaTockaId.Value,
                Termin = podatki.IzbranDatum.Value.AddHours(podatki.IzbranaUra.Value)
            };

            if (roles.Contains("Admin"))
            {
                //Admin lahko ureja uporabnika
                dostava.PodjetjeId = uporabnikIzbire.PodjetjeId;
                dostava.UporabnikId = uporabnikIzbire.Id;
            }

            _krkaRepo.AddDostava(dostava);
            _krkaRepo.SaveChanges();
        }

        public void DeleteDostava(string sifraDostave)
        {
            var d = _krkaRepo.GetDostava(sifraDostave);
            _krkaRepo.DeleteDostava(d.Id);
            _krkaRepo.SaveChanges();
        }

        public void SetStatusDostave(string sifraDostave, StatusDostave status)
        {
            _krkaRepo.SetStatusDostave(sifraDostave, status);
            _krkaRepo.SaveChanges();
        }
    }

    public interface IDostavaDataManager
    {
        Task<IEnumerable<Dostava>> GetAllSeznamDostavAsync(DateTime from, DateTime to, Uporabnik uporabnik);
        Skladisce GetSkladisceFromSifraDostave(string sifraDostave);
        Dostava GetDostava(string sifraDostave);
        Task<string> CheckEditDostavaAsync(Dostava dostava, Uporabnik uporabnik);
        IEnumerable<TerminTockeSkladisca> GetAllZasedeniTermini(DateTime start, DateTime end, int skladisceId);
        Task<IEnumerable<Uporabnik>> GetAllUporabnikiForVnosDostaveAsync();
        Task UrediDostavoAsync(IzberiterminDostaveModel podatki, Uporabnik uporabnikPrijave);
        Task DodajDostavoAsync(IzberiterminDostaveModel podatki, Uporabnik uporabnikPrijave);
        void DeleteDostava(string sifraDostave);
        void SetStatusDostave(string sifraDostave, StatusDostave status);
    }
}
