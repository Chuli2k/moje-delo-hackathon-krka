using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Data
{
    public class Uporabnik :  IdentityUser
    {
        public int PodjetjeId { get; set; }
        public int? SkladisceId { get; set; }
        public HashSet<Dostava> Dostave { get; set; } = new HashSet<Dostava>();
        public Skladisce Skladisce { get; set; }
    }
}
