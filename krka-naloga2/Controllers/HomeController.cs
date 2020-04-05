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
using Microsoft.AspNetCore.Identity;
using krka_naloga2.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace krka_naloga2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKrkaRepo _krkaRepo;
        private readonly UserManager<Uporabnik> _userManager;
        private readonly IDostavaDataManager _dostavaDataManager;

        public HomeController(
            ILogger<HomeController> logger,
            IKrkaRepo krkaRepo,
            UserManager<Uporabnik> userManager,
            IDostavaDataManager dostavaDataManager)
        {
            _logger = logger;
            _krkaRepo = krkaRepo;
            _userManager = userManager;
            _dostavaDataManager = dostavaDataManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Uporabnik")]
        public IActionResult VnesiStDostave()
        {
            return View();
        }

        [HttpPost("VnesiStDostave")]
        [Authorize(Roles = "Admin, Uporabnik")]
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

            var dostavaDb = _krkaRepo.GetDostava(dostava.Sifra);
            if(dostavaDb != null)
            {
                ModelState.AddModelError("Sifra", $"Šifra dostave že obstaja!");
                return View("VnesiStDostave");
            }

            return RedirectToAction("IzberiTermin", new { sifraDostave = dostava.Sifra });
        }

        [HttpGet("/Dostava/{sifraDostave}/IzbiraTermina")]
        [Authorize(Roles = "Admin, Uporabnik")]
        public async Task<IActionResult> IzberiTermin(string sifraDostave)
        {
            //TODO: validacija sifreDostave

            var model = new IzberiterminDostaveModel()
            {
                SifraDostave = sifraDostave,
                SeznamTerminov = new List<IzbiraTerminaModel>()
            };

            var dostava = _krkaRepo.GetDostava(sifraDostave);

            if (dostava != null && dostava.Status == StatusDostave.Potrjen)
            {
                ModelState.AddModelError("SifraDostave", "Šifra dostave je že potrjena!");
                return View(model);
            }

            if (dostava != null)
            {
                model.IzbranDatum = dostava.Termin.Date;
                model.IzbranaUra = dostava.Termin.Hour;
                model.IzbranaTockaId = dostava.TockaSkladiscaId;
                model.IzbranaTockaSifra = dostava.TockaSkladisca.Sifra;
                model.IzbranUporabnikId = dostava.UporabnikId;
            }

            //TODO: preveri, če uporabnik lahko ureja dostavo.

            var skl = _krkaRepo.GetSkladisceByPrefix(sifraDostave[0].ToString());
            if(skl == null)
            {
                ModelState.AddModelError("SifraDostave", "Skladišče ne obstaja");
                return View(model);
            }
            
            model.SkladisceSifra = skl.Sifra;

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

            model.SeznamTerminov = seznam;
            var podjetja = _krkaRepo.GetAllPodjetja();
            var users = new List<Uporabnik>();
            users.AddRange(await _userManager.GetUsersInRoleAsync("Uporabnik"));
            users.AddRange(await _userManager.GetUsersInRoleAsync("Admin"));

            model.Uporabniki = users.Select(t => new SelectListItem() { 
                Value = t.Id.ToString(), 
                Text = $"{t.UserName} ({podjetja.SingleOrDefault(p => p.Id == t.PodjetjeId)?.Naziv})"
            });
            
            return View(model);
        }

        [HttpPost("/Dostava/{sifraDostave}/IzbiraTermina")]
        [Authorize(Roles = "Admin, Uporabnik")]
        public async Task<IActionResult> IzberiTerminPost(string sifraDostave, DateTime? izbranDatum, int? izbranaUra, int? izbranaTockaId, string izbranUporabnikId)
        {
            var uporabnikPrijave = await _userManager.GetUserAsync(User);
            var uporabnikIzbire = await _userManager.FindByIdAsync(izbranUporabnikId);

            //Če si admin moraš izbrat uporabnika
            if (User.IsInRole("Admin") && uporabnikIzbire == null)
                return RedirectToAction("IzberiTermin", new { sifraDostave });

            var dostavaDb = _krkaRepo.GetDostava(sifraDostave);
            if(dostavaDb != null)
            {
                //Urejanje dostave
                //TODO: preverjanje, da uporabnik lahko ureja dostavo
                if (dostavaDb.Status == StatusDostave.Potrjen)
                    return RedirectToAction("IzberiTermin", new { sifraDostave });

                if(User.IsInRole("Admin"))
                {
                    //Lahko ureja uporabnika
                    dostavaDb.UporabnikId = uporabnikIzbire.Id;
                    dostavaDb.PodjetjeId = uporabnikIzbire.PodjetjeId;
                }

                dostavaDb.TockaSkladiscaId = izbranaTockaId.Value;
                dostavaDb.Termin = izbranDatum.Value.AddHours(izbranaUra.Value);

                _krkaRepo.UpdateDostava(dostavaDb);
            }
            else
            {
                //Dodajanje dostave
                var dostava = new Dostava()
                {
                    PodjetjeId = uporabnikPrijave.PodjetjeId,
                    UporabnikId = uporabnikPrijave.Id,
                    Sifra = sifraDostave,
                    TockaSkladiscaId = izbranaTockaId.Value,
                    Termin = izbranDatum.Value.AddHours(izbranaUra.Value)
                };

                if (User.IsInRole("Admin"))
                {
                    //Admin lahko ureja uporabnika
                    dostava.PodjetjeId = uporabnikIzbire.PodjetjeId;
                    dostava.UporabnikId = uporabnikIzbire.Id;
                }

                _krkaRepo.AddDostava(dostava);
            }
            
            _krkaRepo.SaveChanges();
            return RedirectToAction("Porocilo", new { sifraDostave });
        }

        [HttpGet("/Dostava/{sifraDostave}/Porocilo")]
        public async Task<IActionResult> PorociloAsync(string sifraDostave)
        {
            //TODO: preveri, da šifra dostave obstaja

            var dostavaDb = _krkaRepo.GetDostava(sifraDostave);
            var uporabnik = await _userManager.GetUserAsync(User);
            var mailMsg = $"Uporabnik: {uporabnik.UserName}" + Environment.NewLine +
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
        public async Task<IActionResult> TiskajAsync(string sifraDostave)
        {
            //TODO: preveri, da šifra dostave obstaja
            var uporabnik = await _userManager.GetUserAsync(User);
            var model = new PorociloModel()
            {
                UporabnikIme = uporabnik.UserName,
                Dostava = _krkaRepo.GetDostava(sifraDostave)
            };
            return View(model);
        }

        [HttpGet("/Dostava")]
        public async Task<IActionResult> SeznamDostav()
        {
            //var model = _krkaRepo.GetAllDostave(new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            var uporabnik = await _userManager.GetUserAsync(User);
            var model = await _dostavaDataManager.GetAllSeznamDostavAsync(new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), uporabnik);
            return View(model);
        }

        [HttpGet("/Dostava/{sifraDostave}/Delete")]
        [Authorize(Roles = "Admin, Uporabnik")]
        public IActionResult DeleteDostava(string sifraDostave)
        {
            var d = _krkaRepo.GetDostava(sifraDostave);
            _krkaRepo.DeleteDostava(d.Id);
            _krkaRepo.SaveChanges();

            return RedirectToAction("SeznamDostav");
        }

        [HttpGet("/Dostava/{sifraDostave}/Prevzem")]
        [Authorize(Roles = "Admin, Skladiscnik")]
        public IActionResult Prevzem(string sifraDostave)
        {
            var d = _krkaRepo.GetDostava(sifraDostave);

            return View(d);
        }

        [HttpPost("/Dostava/{sifraDostave}/Prevzem")]
        [Authorize(Roles = "Admin, Skladiscnik")]
        public IActionResult PrevzemPotrdi(string sifraDostave)
        {
            var dostavaDb = _krkaRepo.GetDostava(sifraDostave);

            if (dostavaDb == null)
                return RedirectToAction("Prevzem", new { sifraDostave });

            _krkaRepo.SetStatusDostave(sifraDostave, StatusDostave.Potrjen);
            _krkaRepo.SaveChanges();

            return RedirectToAction("Prevzem", new { sifraDostave });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
