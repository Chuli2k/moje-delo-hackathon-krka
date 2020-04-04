using krka_naloga2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Models
{
    public class PorociloModel
    {
        public string UporabnikIme { get; set; }
        public string MailMessage { get; set; }
        public Dostava Dostava { get; set; }
    }
}
