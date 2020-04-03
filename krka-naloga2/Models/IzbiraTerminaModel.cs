using System;
using System.Collections.Generic;

namespace krka_naloga2.Models
{
    public class IzbiraTerminaModel
    {
        public DateTime Datum { get; set; }
        public IList<TockaSkladiscaTerminaModel> SeznamTockSkladisca { get; set; }
        
    }

    
}
