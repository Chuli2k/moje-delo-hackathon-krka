using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Models
{
    public class IzberiterminDostaveModel
    {
        public string SifraDostave { get; set; }
        public DateTime? IzbranDatum { get; set; }
        public int? IzbranaUra { get; set; }
        public string IzbranaTockaSifra { get; set; }
        public int? IzbranaTockaId { get; set; }
        public string IzbranUporabnikId { get; set; }
        public string SkladisceSifra { get; set; }
        public IEnumerable<IzbiraTerminaModel> SeznamTerminov { get; set; }
        public IEnumerable<SelectListItem> Uporabniki { get; set; } = new List<SelectListItem>();
    }

    public class TerminIzbira
    {
        public DateTime? IzbranDatum { get; set; }
        public int? IzbranaUra { get; set; }
        public string IzbranaTockaSifra { get; set; }
    }
}
