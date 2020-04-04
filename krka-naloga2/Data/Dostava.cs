using System;

namespace krka_naloga2.Data
{
    public class Dostava
    {
        public int Id { get; set; }
        public string Sifra { get; set; }
        public DateTime Termin { get; set; }
        public int TockaSkladiscaId { get; set; }
        public int PodjetjeId { get; set; }
        public string UporabnikId { get; set; }
        public TockaSkladisca TockaSkladisca { get; set; }
        public Podjetje Podjetje { get; set; }
        public Uporabnik Uporabnik { get; set; }
    }
}
