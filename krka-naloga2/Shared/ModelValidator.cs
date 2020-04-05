using krka_naloga2.Data;
using krka_naloga2.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Shared
{
    public class ModelValidator : IModelValidator
    {
        private readonly IKrkaRepo _krkaRepo;
        private readonly UserManager<Uporabnik> _userManager;

        public ModelValidator(IKrkaRepo krkaRepo, UserManager<Uporabnik> userManager)
        {
            _krkaRepo = krkaRepo;
            _userManager = userManager;
        }

        public string ValidateSifraDostave(string sifraDostave)
        {
            int stDostave;
            var parsed = int.TryParse(sifraDostave, out stDostave);
            if (!parsed)
            {
                return "Šifra dostave mora biti številka";
            }

            var izbranoSkl = _krkaRepo.GetSkladisceByPrefix(sifraDostave[0].ToString());
            if (izbranoSkl == null)
            {
                return $"Skladišče {sifraDostave[0]} ne obstaja.";
            }

            if (sifraDostave.Length != 4)
            {
                return $"Šifra dostave mora biti štirimestna številka";
            }

            return null;
        }

        public string ValidateVnosSifraDostave(string sifraDostave)
        {
            var err = ValidateSifraDostave(sifraDostave);
            if (!string.IsNullOrEmpty(err)) return err;

            var dostavaDb = _krkaRepo.GetDostava(sifraDostave);
            if (dostavaDb != null)
            {
                return $"Šifra dostave že obstaja!";
            }

            return null;
        }

        public async Task<string> ValidateVnosTermina(IzberiterminDostaveModel podatki, Uporabnik uporabnikPrijave)
        {
            if (!podatki.IzbranDatum.HasValue) return "Izberite termin.";

            var uporabnikIzbire = await _userManager.FindByIdAsync(podatki.IzbranUporabnikId);
            var roles = await _userManager.GetRolesAsync(uporabnikPrijave);

            //Če si admin moraš izbrat uporabnika
            if (roles.Contains("Admin") && uporabnikIzbire == null)
                return "Ni izbran uporabnik.";

            return null;
        }
    }
}
