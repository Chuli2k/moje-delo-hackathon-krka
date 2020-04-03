using System;
using System.Collections.Generic;

namespace krka_naloga2.Data
{
    public class Skladisce
    {
        public int Id { get; set; }
        public string Sifra { get; set; }

        public HashSet<TockaSkladisca> TockeSkladisca { get; set; }
    }
}
