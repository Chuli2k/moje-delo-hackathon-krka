using System;
using System.Collections.Generic;

namespace krka_naloga2.Data
{
    public class Podjetje
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Sifra { get; set; }
        public HashSet<Dostava> Dostave { get; set; } = new HashSet<Dostava>();
    }
}
