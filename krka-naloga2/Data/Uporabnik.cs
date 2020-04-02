using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace krka_naloga2.Data
{
    public class Uporabnik :  IdentityUser
    {
        public string Podjetje { get; set; }
    }
}
