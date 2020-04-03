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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        [HttpPost]
        public IActionResult VnesiStDostavePost(StDostaveInputModel dostava)
        {
            return RedirectToAction("IzberiTermin", new { sifraDostave = dostava.Sifra });
        }

        [HttpGet("/Dostava/{sifraDostave}/IzbiraTermina")]
        public IActionResult IzberiTermin(string sifraDostave)
        {
            var seznam = new List<IzbiraTerminaModel>();
            for (int dan = 0; dan < 7; dan++)
            {
                var danObj = new IzbiraTerminaModel()
                {
                    Datum = new DateTime(2020, 3, 30).AddDays(dan),
                    SeznamTockSkladisca = new List<TockaSkladiscaTerminaModel>()
                };

                for (int tocka = 1; tocka < 6; tocka++)
                {
                    var tockaObj = new TockaSkladiscaTerminaModel()
                    {
                        SifraTockeSkladisca = tocka.ToString(),
                        SeznamUreTermina = new List<UraTerminaModel>()
                    };

                    for (int ura = 0; ura < 24; ura++)
                    {
                        var r = new Random().Next(0, 100);
                        var uraObj = new UraTerminaModel()
                        {
                            Ura = ura,
                            JeProst = r%2 == 0
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
                SeznaTerminov = seznam
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
