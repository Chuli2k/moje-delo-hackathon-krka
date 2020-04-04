using System;
using System.Collections.Generic;

namespace krka_naloga2.Data
{
    public class TockaSkladisca
    {
        public int Id { get; set; }
        public string Sifra { get; set; }
        public int SkladisceId { get; set; }
        public Skladisce Skladisce { get; set; }
        public HashSet<Dostava> Dostave { get; set; } = new HashSet<Dostava>();
    }
}
