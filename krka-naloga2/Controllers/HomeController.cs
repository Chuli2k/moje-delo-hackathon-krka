using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using krka_naloga2.Models;
using Microsoft.AspNetCore.Authorization;
using krka_naloga2.Data;

namespace krka_naloga2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKrkaRepo _krkaRepo;

        public HomeController(
            ILogger<HomeController> logger,
            IKrkaRepo krkaRepo)
        {
            _logger = logger;
            _krkaRepo = krkaRepo;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VnesiStDostave()
        {
            return View();
        }

        [HttpPost("VnesiStDostave")]
        public IActionResult VnesiStDostavePost(StDostaveInputModel dostava)
        {
            int stDostave;
            var parsed = int.TryParse(dostava.Sifra, out stDostave);
            if (!parsed)
            {
                ModelState.AddModelError("Sifra", "vnesite številko");
                return View("VnesiStDostave");
            }

            var izbranoSkl = _krkaRepo.GetSkladisceByPrefix(dostava.Sifra[0].ToString());
            if (izbranoSkl == null)
            {
                ModelState.AddModelError("Sifra", $"Skladišče {dostava.Sifra[0]} ne obstaja.");
                return View("VnesiStDostave");
            }

            if(dostava.Sifra.Length != 4)
            {
                ModelState.AddModelError("Sifra", $"Šifra dostave mora biti štirimestna številka");
                return View("VnesiStDostave");
            }

            return RedirectToAction("IzberiTermin", new { sifraDostave = dostava.Sifra });
        }

        [HttpGet("/Dostava/{sifraDostave}/IzbiraTermina")]
        public IActionResult IzberiTermin(string sifraDostave)
        {
            //TODO: validacija sifreDostave

            var skl = _krkaRepo.GetSkladisceByPrefix(sifraDostave[0].ToString());

            var zacetniDatum = DateTime.Now.Date;
            var stDni = 7;

            var tockeSkladisca = _krkaRepo.GetAllTockeSkladisca(skl.Id);
            var dostave = _krkaRepo.GetAllDostave(zacetniDatum, zacetniDatum.AddDays(stDni), skl.Id);

            var seznam = new List<IzbiraTerminaModel>();
            for (int dan = 0; dan < stDni; dan++)
            {
                var danObj = new IzbiraTerminaModel()
                {
                    Datum = zacetniDatum.AddDays(dan),
                    SeznamTockSkladisca = new List<TockaSkladiscaTerminaModel>()
                };

                foreach (var tocka in tockeSkladisca.OrderBy(t => t.Sifra))
                {
                    var tockaObj = new TockaSkladiscaTerminaModel()
                    {
                        SifraTockeSkladisca = tocka.Sifra,
                        TockaSkladiscaId = tocka.Id,
                        SeznamUreTermina = new List<UraTerminaModel>()
                    };

                    for (int ura = 0; ura < 24; ura++)
                    {
                        var r = new Random().Next(0, 100);
                        var uraObj = new UraTerminaModel()
                        {
                            Ura = ura,
                            JeProst = !dostave.Any(t => t.Termin.Date == danObj.Datum && t.Termin.Hour == ura && t.TockaSkladiscaId == tocka.Id),
                            JeIzbran = false
                        };

                        tockaObj.SeznamUreTermina.Add(uraObj);
                    }

                    danObj.SeznamTockSkladisca.Add(tockaObj);
                }

                seznam.Add(danObj);
            }

            var model = new IzberiterminDostaveModel()
            {
                SifraDostave = sifraDostave,
                SeznamTerminov = seznam,
                SkladisceSifra = skl.Sifra
            };
            return View(model);
        }

        [HttpPost("/Dostava/{sifraDostave}/IzbiraTermina")]
        public IActionResult IzberiTerminPost(string sifraDostave, DateTime? izbranDatum, int? izbranaUra, int? izbranaTockaSifra) //TODO: Poglej kak uporabit posebaj class namesto seznama parametrov.
        {
            var dostava = new Dostava()
            {
                PodjetjeId = 2,
                Sifra = sifraDostave,
                TockaSkladiscaId = izbranaTockaSifra.Value,
                Termin = izbranDatum.Value.AddHours(izbranaUra.Value)
            };
            _krkaRepo.AddDostava(dostava);
            _krkaRepo.SaveChanges();
            return RedirectToAction("Porocilo", new { sifraDostave });
        }

        [HttpGet("/Dostava/{sifraDostave}/Porocilo")]
        public IActionResult Porocilo(string sifraDostave)
        {
            //TODO: preveri, da šifra dostave obstaja

            var dostavaDb = _krkaRepo.GetDostava(sifraDostave);

            var mailMsg = $"Uporabnik: {User.Identity.Name}" + Environment.NewLine +
                        $"Podjetje: {dostavaDb.Podjetje.Naziv}" + Environment.NewLine +
                        $"Skladišče: {dostavaDb.TockaSkladisca.Skladisce.Sifra}" + Environment.NewLine +
                        $"Točka skladišča: {dostavaDb.TockaSkladisca.Sifra}" + Environment.NewLine +
                        $"Termin: {dostavaDb.Termin}" + Environment.NewLine;

            mailMsg = mailMsg.Replace(Environment.NewLine, "%0D%0A");
            mailMsg = mailMsg.Replace(" ", "%20");

            var model = new PorociloModel()
            {
                UporabnikIme = User.Identity.Name,
                MailMessage = mailMsg,
                Dostava = dostavaDb
            };
            return View(model);
        }

        [HttpGet("/Dostava/{sifraDostave}/Tiskaj")]
        public IActionResult Tiskaj(string sifraDostave)
        {
            //TODO: preveri, da šifra dostave obstaja

            var model = new PorociloModel()
            {
                UporabnikIme = User.Identity.Name,
                Dostava = _krkaRepo.GetDostava(sifraDostave)
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
