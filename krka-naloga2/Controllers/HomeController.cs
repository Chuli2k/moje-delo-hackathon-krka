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
        private readonly UserManager<Uporabnik> _userManager;
        private readonly IDostavaDataManager _dostavaDataManager;
        private readonly IModelValidator _modelValidator;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<Uporabnik> userManager,
            IDostavaDataManager dostavaDataManager,
            IModelValidator modelValidator)
        {
            _logger = logger;
            _userManager = userManager;
            _dostavaDataManager = dostavaDataManager;
            _modelValidator = modelValidator;
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
            var modelError = _modelValidator.ValidateVnosSifraDostave(dostava.Sifra);
            if(!string.IsNullOrEmpty(modelError))
            {
                ModelState.AddModelError("Sifra", modelError);
                return View("VnesiStDostave");
            }

            return RedirectToAction("IzberiTermin", new { sifraDostave = dostava.Sifra });
        }

        [HttpGet("/Dostava/{sifraDostave}/IzbiraTermina")]
        [Authorize(Roles = "Admin, Uporabnik")]
        public async Task<IActionResult> IzberiTermin(string sifraDostave)
        {
            var model = new IzberiterminDostaveModel()
            {
                SifraDostave = sifraDostave,
                SeznamTerminov = new List<IzbiraTerminaModel>()
            };

            //Validiraj šifro dostave
            var modelError = _modelValidator.ValidateSifraDostave(sifraDostave);
            if (!string.IsNullOrEmpty(modelError))
            {
                ModelState.AddModelError("SifraDostave", modelError);
                return View(model);
            }

            var dostava = _dostavaDataManager.GetDostava(sifraDostave);

            if (dostava != null)
            {
                var dostavaEditErrors = await _dostavaDataManager.CheckEditDostavaAsync(dostava, await _userManager.GetUserAsync(User));
                if (!string.IsNullOrEmpty(dostavaEditErrors))
                {
                    ModelState.AddModelError("SifraDostave", dostavaEditErrors);
                    return View(model);
                }

                model.IzbranDatum = dostava.Termin.Date;
                model.IzbranaUra = dostava.Termin.Hour;
                model.IzbranaTockaId = dostava.TockaSkladiscaId;
                model.IzbranaTockaSifra = dostava.TockaSkladisca.Sifra;
                model.IzbranUporabnikId = dostava.UporabnikId;
            }

            var skl = _dostavaDataManager.GetSkladisceFromSifraDostave(sifraDostave);
            if(skl == null)
            {
                ModelState.AddModelError("SifraDostave", "Skladišče ne obstaja");
                return View(model);
            }
            
            model.SkladisceSifra = skl.Sifra;

            var zacetniDatum = DateTime.Now.Date;
            var stDni = 7;

            var dostave = _dostavaDataManager.GetAllZasedeniTermini(zacetniDatum, zacetniDatum.AddDays(stDni), skl.Id);
            var seznam = new List<IzbiraTerminaModel>();
            for (int dan = 0; dan < stDni; dan++)
            {
                var danObj = new IzbiraTerminaModel()
                {
                    Datum = zacetniDatum.AddDays(dan),
                    SeznamTockSkladisca = new List<TockaSkladiscaTerminaModel>()
                };

                foreach (var tocka in skl.TockeSkladisca.OrderBy(t => t.Sifra))
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
            var users = await _dostavaDataManager.GetAllUporabnikiForVnosDostaveAsync();

            model.Uporabniki = users.Select(t => new SelectListItem() { 
                Value = t.Id.ToString(), 
                Text = $"{t.UserName} ({t.Podjetje?.Naziv ?? "Brez"})"
            });
            
            return View(model);
        }

        [HttpPost("/Dostava/{sifraDostave}/IzbiraTermina")]
        [Authorize(Roles = "Admin, Uporabnik")]
        public async Task<IActionResult> IzberiTerminPost([FromRoute] string sifraDostave, [FromForm] IzberiterminDostaveModel podatki)
        {
            //Preveri šifro dostave
            var error = _modelValidator.ValidateSifraDostave(sifraDostave);
            if(!string.IsNullOrEmpty(error))
                return RedirectToAction("IzberiTermin", new { sifraDostave });

            var uporabnikPrijave = await _userManager.GetUserAsync(User);
            
            error = await _modelValidator.ValidateVnosTermina(podatki, uporabnikPrijave);
            if (!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError("SifraDostave", error);
                return View(podatki);
            }

            var dostavaDb = _dostavaDataManager.GetDostava(sifraDostave);

            if (dostavaDb != null)
            {
                var dostavaEditErrors = await _dostavaDataManager.CheckEditDostavaAsync(dostavaDb, uporabnikPrijave);
                if (!string.IsNullOrEmpty(dostavaEditErrors))
                {
                    ModelState.AddModelError("SifraDostave", dostavaEditErrors);
                    return View(podatki);
                }

                await _dostavaDataManager.UrediDostavoAsync(podatki, uporabnikPrijave);
            }
            else
            {
                await _dostavaDataManager.DodajDostavoAsync(podatki, uporabnikPrijave);
            }

            return RedirectToAction("Porocilo", new { sifraDostave });
        }

        [HttpGet("/Dostava/{sifraDostave}/Porocilo")]
        public async Task<IActionResult> PorociloAsync(string sifraDostave)
        {
            var dostavaDb = _dostavaDataManager.GetDostava(sifraDostave);
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
            var uporabnik = await _userManager.GetUserAsync(User);
            var model = new PorociloModel()
            {
                UporabnikIme = uporabnik.UserName,
                Dostava = _dostavaDataManager.GetDostava(sifraDostave)
        };
            return View(model);
        }

        [HttpGet("/Dostava")]
        public async Task<IActionResult> SeznamDostav()
        {
            var uporabnik = await _userManager.GetUserAsync(User);
            var model = await _dostavaDataManager.GetAllSeznamDostavAsync(new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), uporabnik);
            return View(model);
        }

        [HttpGet("/Dostava/{sifraDostave}/Delete")]
        [Authorize(Roles = "Admin, Uporabnik")]
        public IActionResult DeleteDostava(string sifraDostave)
        {
            _dostavaDataManager.DeleteDostava(sifraDostave);

            return RedirectToAction("SeznamDostav");
        }

        [HttpGet("/Dostava/{sifraDostave}/Prevzem")]
        [Authorize(Roles = "Admin, Skladiscnik")]
        public IActionResult Prevzem(string sifraDostave)
        {
            var d = _dostavaDataManager.GetDostava(sifraDostave);

            return View(d);
        }

        [HttpPost("/Dostava/{sifraDostave}/Prevzem")]
        [Authorize(Roles = "Admin, Skladiscnik")]
        public IActionResult PrevzemPotrdi(string sifraDostave)
        {
            var dostavaDb = _dostavaDataManager.GetDostava(sifraDostave);

            if (dostavaDb == null)
                return RedirectToAction("Prevzem", new { sifraDostave });

            _dostavaDataManager.SetStatusDostave(sifraDostave, StatusDostave.Potrjen);

            return RedirectToAction("Prevzem", new { sifraDostave });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
